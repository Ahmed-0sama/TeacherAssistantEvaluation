using Business_Access.Interfaces;
using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Dtos.VPGSEvaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Access.Services
{
    public class VPGSEvaluationService : IVPGSEvaluation
    {
        private readonly SrsDbContext _db;
        public VPGSEvaluationService(SrsDbContext db)
        {
            _db = db;
        }

        public async Task<int> CreateVpgsEvaluationAsync(CreateVpgsEvaluationDto evaluationDto)
        {
            if (evaluationDto == null)
                throw new ArgumentNullException(nameof(evaluationDto));

            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var evaluation = await _db.Evaluations
                    .Include(e => e.Status)
                    .FirstOrDefaultAsync(e => e.EvaluationId == evaluationDto.EvaluationId);

                if (evaluation == null)
                    throw new KeyNotFoundException($"Evaluation with ID {evaluationDto.EvaluationId} not found");

                // Check if VPGS evaluation already exists for this evaluation
                var existingVpgsEval = await _db.VpgsEvaluations
                    .FirstOrDefaultAsync(ve => ve.EvaluationId == evaluationDto.EvaluationId);

                if (existingVpgsEval != null)
                    throw new InvalidOperationException($"VPGS evaluation already exists for evaluation ID {evaluationDto.EvaluationId}");

                // Create new VPGS evaluation
                var vpgsEvaluation = new VpgsEvaluation
                {
                    EvaluationId = evaluationDto.EvaluationId,
                    ScientificScore = evaluationDto.ScientificScore,
                    StatusId= 2 // Set initial status based on evaluation status

                };

                _db.VpgsEvaluations.Add(vpgsEvaluation);
                await _db.SaveChangesAsync();

                await transaction.CommitAsync();

                return vpgsEvaluation.VpgsevalId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<VpgsEvaluationResponseDto> GetVpgsEvaluationByIdAsync(int vpgsevalId)
        {
            if (vpgsevalId <= 0)
                throw new ArgumentException("Invalid VPGS evaluation ID", nameof(vpgsevalId));

            var vpgsEval = await _db.VpgsEvaluations
                .Include(ve => ve.Evaluation)
                    .ThenInclude(e => e.Period)
                .Include(ve => ve.Evaluation)
                    .ThenInclude(e => e.Status)
                .FirstOrDefaultAsync(ve => ve.VpgsevalId == vpgsevalId);

            if (vpgsEval == null)
                throw new KeyNotFoundException($"VPGS evaluation with ID {vpgsevalId} not found");

            return MapToResponseDto(vpgsEval);
        }
        public async Task<VpgsEvaluationResponseDto?> GetVpgsEvaluationByEvaluationIdAsync(int evaluationId)
        {
            if (evaluationId <= 0)
                throw new ArgumentException("Invalid evaluation ID", nameof(evaluationId));

            var vpgsEval = await _db.VpgsEvaluations
                .Include(ve => ve.Evaluation)
                    .ThenInclude(e => e.Period)
                .Include(ve => ve.Evaluation)
                    .ThenInclude(e => e.Status)
                .Include(ve => ve.Evaluation)
                    .ThenInclude(e => e.Tasubmission) // This is correct - loads the TA submission
                .FirstOrDefaultAsync(ve => ve.EvaluationId == evaluationId);

            if (vpgsEval == null)
                return null;

            return MapToResponseDto(vpgsEval);
        }
        public async Task UpdateVpgsEvaluationAsync(int evaluationId, UpdateVpgsEvaluationDto evaluationDto)
        {
            if (evaluationDto == null)
                throw new ArgumentNullException(nameof(evaluationDto));

            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var vpgsEval = await _db.VpgsEvaluations
                    .Include(ve => ve.Evaluation)
                    .FirstOrDefaultAsync(ve => ve.EvaluationId == evaluationId);

                if (vpgsEval == null)
                    throw new KeyNotFoundException($"VPGS evaluation for evaluation ID {evaluationId} not found");

                // Check if evaluation can still be edited
                // Assuming status IDs: 1=Draft, 2=Submitted, etc. Adjust based on your workflow
                if (vpgsEval.Evaluation.StatusId > 6)
                    throw new InvalidOperationException("Cannot update VPGS evaluation after final approval stage");

                // Update the scientific score
                vpgsEval.ScientificScore = evaluationDto.ScientificScore;

                _db.VpgsEvaluations.Update(vpgsEval);
                await _db.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<bool> VpgsEvaluationExistsAsync(int evaluationId)
        {
            if (evaluationId <= 0)
                return false;

            return await _db.VpgsEvaluations
                .AnyAsync(ve => ve.EvaluationId == evaluationId);
        }
        public async Task<IEnumerable<VpgsEvaluationResponseDto>> GetVpgsEvaluationsByPeriodAsync(int periodId)
        {
            if (periodId <= 0)
                throw new ArgumentException("Invalid period ID", nameof(periodId));

            var vpgsEvaluations = await _db.VpgsEvaluations
                .Include(ve => ve.Evaluation.Period)
                .Include(ve => ve.Evaluation.Status)
                .Where(ve => ve.Evaluation.PeriodId == periodId)
                .OrderBy(ve => ve.Evaluation.TaEmployeeId)
                .ToListAsync();

            return vpgsEvaluations.Select(ve => MapToResponseDto(ve)).ToList();
        }
        private VpgsEvaluationResponseDto MapToResponseDto(VpgsEvaluation vpgsEval)
        {
            return new VpgsEvaluationResponseDto
            {
                VpgsevalId = vpgsEval.VpgsevalId,
                EvaluationId = vpgsEval.EvaluationId,
                ScientificScore = vpgsEval.ScientificScore,
                TAEmployeeId = vpgsEval.Evaluation.TaEmployeeId,
                PeriodName = vpgsEval.Evaluation.Period?.PeriodName ?? "Unknown Period",
                StatusName = vpgsEval.Evaluation.Status?.StatusName ?? "Unknown Status",
            };
        }
    }
}
