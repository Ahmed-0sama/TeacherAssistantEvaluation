using Business_Access.Interfaces;

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
        private readonly IExternalApiService _externalApiService;
        public VPGSEvaluationService(SrsDbContext db , IExternalApiService externalApiService)
        {
            _db = db;
            _externalApiService = externalApiService;
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

                var existingVpgsEval = await _db.VpgsEvaluations
                    .FirstOrDefaultAsync(ve => ve.EvaluationId == evaluationDto.EvaluationId);

                if (existingVpgsEval != null)
                    throw new InvalidOperationException($"VPGS evaluation already exists for evaluation ID {evaluationDto.EvaluationId}");

                var vpgsEvaluation = new VpgsEvaluation
                {
                    EvaluationId = evaluationDto.EvaluationId,
                    ScientificScore = evaluationDto.ScientificScore,
                    StatusId= 2

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
                    .ThenInclude(e => e.Tasubmission) 
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
        public async Task<List<UserDataDto>> GetGTAsForVPGSAsync(int periodId, int supervisorId, DateOnly startDate)
        {
            try
            {
                Console.WriteLine($"Loading GTAs for VPGS - Period: {periodId}, Supervisor: {supervisorId}");

                var gtaList = await _externalApiService.GetGTAListAsync(supervisorId, startDate);

                if (gtaList == null || !gtaList.Any())
                {
                    Console.WriteLine("No GTAs found from external API");
                    return new List<UserDataDto>();
                }

                Console.WriteLine($"Loaded {gtaList.Count} GTAs from external API");

                var evaluations = await _db.Evaluations
                    .Include(e => e.Period)
                    .Include(e => e.Status)
                    .Include(e => e.Tasubmission)
                    .Where(e => e.PeriodId == periodId)
                    .ToListAsync();

                var evaluationMap = evaluations.ToDictionary(e => e.TaEmployeeId);

                Console.WriteLine($"Found {evaluations.Count} evaluations in database");

                var vpgsEvaluations = await _db.VpgsEvaluations
                    .Where(ve => evaluations.Select(e => e.EvaluationId).Contains(ve.EvaluationId ?? 0))
                    .ToListAsync();

                var vpgsMap = vpgsEvaluations.ToDictionary(ve => ve.EvaluationId ?? 0);

                Console.WriteLine($"Found {vpgsEvaluations.Count} VPGS evaluations");

                foreach (var gta in gtaList)
                {
                    if (evaluationMap.TryGetValue(gta.employeeId, out var evaluation))
                    {
                        // GTA has an evaluation
                        gta.EvaluationId = evaluation.EvaluationId;
                        gta.statusid = evaluation.StatusId;
                        gta.HasSubmitted = evaluation.StatusId >= 2; // StatusId >= 2 means submitted
                        gta.HasVpgsEvaluation = vpgsMap.ContainsKey(evaluation.EvaluationId);

                        Console.WriteLine($" Enriched GTA: {gta.employeeName} - StatusId: {evaluation.StatusId} - HasSubmitted: {gta.HasSubmitted} - HasVPGS: {gta.HasVpgsEvaluation}");
                    }
                    else
                    {
                        // GTA doesn't have an evaluation yet
                        gta.EvaluationId = 0;
                        gta.statusid = 0; // No evaluation
                        gta.HasSubmitted = false;
                        gta.HasVpgsEvaluation = false;

                        Console.WriteLine($" GTA {gta.employeeName} has no evaluation yet");
                    }

                    gta.EmployeeNumber = gta.employeeId;
                }

                Console.WriteLine($"Total GTAs processed: {gtaList.Count}");

                return gtaList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error in GetGTAsForVPGSAsync: {ex.Message}");
                throw new Exception($"Failed to get GTAs for VPGS: {ex.Message}", ex);
            }
        }
        private VpgsEvaluationResponseDto MapToResponseDto(VpgsEvaluation vpgsEval)
        {
            return new VpgsEvaluationResponseDto
            {
                VpgsevalId = vpgsEval.VpgsevalId,
                EvaluationId = vpgsEval.EvaluationId??0,
                ScientificScore = vpgsEval.ScientificScore,
                TAEmployeeId = vpgsEval.Evaluation.TaEmployeeId,
                PeriodName = vpgsEval.Evaluation.Period?.PeriodName ?? "Unknown Period",
                StatusName = vpgsEval.Evaluation.Status?.StatusName ?? "Unknown Status",
            };
        }
    }
}
