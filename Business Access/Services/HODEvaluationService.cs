using Business_Access.Interfaces;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos.HODEvaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Access.Services
{
    public class HODEvaluationService : IHODEvaluation
    {
        private readonly SrsDbContext _db;
        public HODEvaluationService(SrsDbContext db)
        {
            _db = db;
        }
        public async Task<int> CreateHodEvaluationAsync(CreateHodEvaluationDto dto)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                // Validate evaluation exists
                var evaluation = await _db.Evaluations.FindAsync(dto.EvaluationId);
                if (evaluation == null)
                    throw new Exception("Evaluation not found");

                // Check if HOD evaluation already exists
                var existingEval = await _db.Hodevaluations
                    .AnyAsync(h => h.EvaluationId == dto.EvaluationId);

                if (existingEval)
                    throw new Exception("HOD evaluation already exists for this evaluation");

                // Create HOD evaluations for each criterion
                foreach (var criterionRating in dto.CriterionRatings)
                {
                    var hodEval = new Hodevaluation
                    {
                        EvaluationId = dto.EvaluationId,
                        CriterionId = criterionRating.CriterionId,
                        RatingId = criterionRating.RatingId,
                        StatusId = 5
                    };

                    _db.Hodevaluations.Add(hodEval);
                }

                // Update evaluation with HOD comments
                evaluation.HodStrengths = dto.HodStrengths;
                evaluation.HodWeaknesses = dto.HodWeaknesses;
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return dto.EvaluationId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<HodEvaluationResponseDto?> GetHodEvaluationByEvaluationIdAsync(int evaluationId)
        {
            var evaluation = await _db.Evaluations
                .Include(e => e.Period)
                .Include(e => e.Status)
                .Include(e => e.Hodevaluations)
                    .ThenInclude(h => h.Criterion)
                .Include(e => e.Hodevaluations)
                    .ThenInclude(h => h.Rating)
                .FirstOrDefaultAsync(e => e.EvaluationId == evaluationId);

            if (evaluation == null)
                return null;

            var hodEvaluations = evaluation.Hodevaluations.Select(h => new HodEvaluationDto
            {
                HodevalId = h.HodevalId,
                EvaluationId = h.EvaluationId,
                CriterionId = h.CriterionId,
                CriterionName = h.Criterion.CriterionName,
                CriterionType = h.Criterion.CriterionType,
                RatingId = h.RatingId,
                RatingName = h.Rating.RatingName,
                ScoreValue = h.Rating.ScoreValue
            }).ToList();
            var totalScore = hodEvaluations.Sum(h => h.ScoreValue);
            var maxScore = hodEvaluations.Count * hodEvaluations.Max(h => h.ScoreValue);
            

            return new HodEvaluationResponseDto
            {
                EvaluationId = evaluation.EvaluationId,
                TaName = $"TA #{evaluation.TaEmployeeId}",
                TaEmployeeId = evaluation.TaEmployeeId,
                PeriodName = evaluation.Period.PeriodName,
                StatusName = evaluation.Status.StatusName,
                StatusId = evaluation.StatusId,
                Evaluations = hodEvaluations,
                HodStrengths = evaluation.HodStrengths,
                HodWeaknesses = evaluation.HodWeaknesses,
                TotalScore = totalScore,
                MaxScore = maxScore,
            };
        }

        public async Task<List<HodEvaluationResponseDto>> GetHodEvaluationsByPeriodAsync(int periodId)
        {
            var evaluations = await _db.Evaluations
                .Include(e => e.Period)
                .Include(e => e.Status)
                .Include(e => e.Hodevaluations)
                    .ThenInclude(h => h.Criterion)
                .Include(e => e.Hodevaluations)
                    .ThenInclude(h => h.Rating)
                .Where(e => e.PeriodId == periodId)
                .ToListAsync();

            return evaluations.Select(evaluation =>
            {
                var hodEvaluations = evaluation.Hodevaluations.Select(h => new HodEvaluationDto
                {
                    HodevalId = h.HodevalId,
                    EvaluationId = h.EvaluationId,
                    CriterionId = h.CriterionId,
                    CriterionName = h.Criterion.CriterionName,
                    CriterionType = h.Criterion.CriterionType,
                    RatingId = h.RatingId,
                    RatingName = h.Rating.RatingName,
                    ScoreValue = h.Rating.ScoreValue,
                    statusid = h.StatusId  
                }).ToList();

                var totalScore = hodEvaluations.Any() ? hodEvaluations.Sum(h => h.ScoreValue) : 0;
                var maxScore = hodEvaluations.Any()
                    ? hodEvaluations.Count * hodEvaluations.Max(h => h.ScoreValue)
                    : 0;

                return new HodEvaluationResponseDto
                {
                    EvaluationId = evaluation.EvaluationId,
                    TaName = $"TA #{evaluation.TaEmployeeId}",
                    TaEmployeeId = evaluation.TaEmployeeId,
                    PeriodName = evaluation.Period.PeriodName,
                    StatusName = evaluation.Status.StatusName,
                    StatusId = evaluation.StatusId,  // ADD THIS
                    Evaluations = hodEvaluations,
                    HodStrengths = evaluation.HodStrengths,
                    HodWeaknesses = evaluation.HodWeaknesses,
                    TotalScore = totalScore,
                    MaxScore = maxScore
                };
            }).ToList();
        }

        public async Task UpdateHodEvaluationAsync(int evaluationId, UpdateHodEvaluationDto dto)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var evaluation = await _db.Evaluations.FindAsync(evaluationId);
                if (evaluation == null)
                    throw new Exception("Evaluation not found");

                // Remove existing HOD evaluations
                var existingEvals = await _db.Hodevaluations
                    .Where(h => h.EvaluationId == evaluationId)
                    .ToListAsync();

                _db.Hodevaluations.RemoveRange(existingEvals);

                // Add new HOD evaluations
                foreach (var criterionRating in dto.CriterionRatings)
                {
                    var hodEval = new Hodevaluation
                    {
                        EvaluationId = evaluationId,
                        CriterionId = criterionRating.CriterionId,
                        RatingId = criterionRating.RatingId
                    };

                    _db.Hodevaluations.Add(hodEval);
                }

                // Update evaluation comments
                evaluation.HodStrengths = dto.HodStrengths;
                evaluation.HodWeaknesses = dto.HodWeaknesses;
                evaluation.StatusId = 5; // Assuming 5 is the status for updated HOD evaluation
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
            public async Task<bool> HasHodEvaluationAsync(int evaluationId)
        {
            return await _db.Hodevaluations
                .AnyAsync(h => h.EvaluationId == evaluationId);
        }
    
    }
}
