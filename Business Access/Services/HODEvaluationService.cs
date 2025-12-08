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
                    // Get criterion to determine type
                    var criterion = await _db.HodevaluationCriteria
                        .FindAsync(criterionRating.CriterionId);

                    if (criterion == null)
                        throw new Exception($"Criterion {criterionRating.CriterionId} not found");

                    var hodEval = new Hodevaluation
                    {
                        EvaluationId = dto.EvaluationId,
                        CriterionId = criterionRating.CriterionId,
                        RatingId = criterionRating.RatingId
                    };

                    _db.Hodevaluations.Add(hodEval);
                }

                // Update evaluation status and comments
                evaluation.StatusId = 5; // Completed HOD evaluation
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

        public async Task<HodEvaluationResponseDto> GetHodEvaluationAsync(int evaluationId)
        {
            var evaluations = await _db.Hodevaluations
                .Where(h => h.EvaluationId == evaluationId)
                .Include(h => h.Criterion)
                .Include(h => h.Rating)
                .ToListAsync();

            if (!evaluations.Any())
                throw new Exception("HOD evaluation not found");

            var evaluation = await _db.Evaluations
                //.Include(e => e.TaEmployeeId)
                .FirstOrDefaultAsync(e => e.EvaluationId == evaluationId);

            if (evaluation == null)
                throw new Exception("Evaluation not found");

            // Calculate totals for each section
            var teachingActivitiesTotal = evaluations
                .Where(e => e.Criterion.CriterionType == "DirectTeaching")
                .Sum(e => MapScoreToPoints(e.Rating.ScoreValue, "DirectTeaching"));

            var studentActivitiesTotal = evaluations
                .Where(e => e.Criterion.CriterionType == "StudentActivities")
                .Sum(e => MapScoreToPoints(e.Rating.ScoreValue, "StudentActivities"));

            var personalTraitsTotal = evaluations
                .Where(e => e.Criterion.CriterionType == "PersonalTraits")
                .Sum(e => MapScoreToPoints(e.Rating.ScoreValue, "PersonalTraits"));

            var administrativeTotal = evaluations
                .Where(e => e.Criterion.CriterionType == "Administrative" ||
                           e.Criterion.CriterionType == "AdministrativeTotal")
                .Sum(e => MapScoreToPoints(e.Rating.ScoreValue, "Administrative"));

            // Map evaluations to DTOs
            var evaluationDtos = evaluations.Select(e => new HodEvaluationItemDto
            {
                CriterionId = e.CriterionId,
                CriterionName = e.Criterion.CriterionName,
                CriterionType = e.Criterion.CriterionType,
                RatingId = e.RatingId,
                RatingName = e.Rating.RatingName,
                ScoreValue = e.Rating.ScoreValue,
                ActualPoints = MapScoreToPoints(e.Rating.ScoreValue, e.Criterion.CriterionType)
            }).ToList();

            var response = new HodEvaluationResponseDto
            {
                EvaluationId = evaluationId,
                TaName = $"TA #{evaluation.TaEmployeeId}",
                TaEmployeeId = evaluation.TaEmployeeId,
                StatusId=evaluation.StatusId,
                Evaluations = evaluationDtos,
                TeachingActivitiesTotal = teachingActivitiesTotal,
                StudentActivitiesTotal = studentActivitiesTotal,
                PersonalTraitsTotal = personalTraitsTotal,
                AdministrativeTotal = administrativeTotal,
                TotalScore = teachingActivitiesTotal + studentActivitiesTotal +
                            personalTraitsTotal + administrativeTotal,
                MaxScore = 40, // 10 + 10 + 10 + 10
                HodStrengths = evaluation.HodStrengths,
                HodWeaknesses = evaluation.HodWeaknesses
            };

            return response;
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

            var result = evaluations.Select(evaluation =>
            {
                // Map each HOD evaluation row to the SAME DTO structure you use in GetHodEvaluationAsync
                var hodEvaluationItems = evaluation.Hodevaluations.Select(h => new HodEvaluationItemDto
                {
                    CriterionId = h.CriterionId,
                    CriterionName = h.Criterion.CriterionName,
                    CriterionType = h.Criterion.CriterionType,
                    RatingId = h.RatingId,
                    RatingName = h.Rating.RatingName,
                    ScoreValue = h.Rating.ScoreValue,
                    ActualPoints = MapScoreToPoints(h.Rating.ScoreValue, h.Criterion.CriterionType)
                }).ToList();

                // Section totals using ActualPoints (NOT raw ScoreValue)
                var teachingActivitiesTotal = hodEvaluationItems
                    .Where(x => x.CriterionType == "DirectTeaching")
                    .Sum(x => x.ActualPoints);

                var studentActivitiesTotal = hodEvaluationItems
                    .Where(x => x.CriterionType == "StudentActivities")
                    .Sum(x => x.ActualPoints);

                var personalTraitsTotal = hodEvaluationItems
                    .Where(x => x.CriterionType == "PersonalTraits")
                    .Sum(x => x.ActualPoints);

                var administrativeTotal = hodEvaluationItems
                    .Where(x => x.CriterionType == "Administrative"
                             || x.CriterionType == "AdministrativeTotal")
                    .Sum(x => x.ActualPoints);

                var totalScore = teachingActivitiesTotal
                               + studentActivitiesTotal
                               + personalTraitsTotal
                               + administrativeTotal;

                // Full HOD part = 40 (10 + 10 + 10 + 10)
                const decimal maxScore = 40m;

                return new HodEvaluationResponseDto
                {
                    EvaluationId = evaluation.EvaluationId,
                    TaName = $"TA #{evaluation.TaEmployeeId}",
                    TaEmployeeId = evaluation.TaEmployeeId,
                    StatusId = evaluation.StatusId,
                    Evaluations = hodEvaluationItems,   // <-- THIS is the fix
                    TeachingActivitiesTotal = teachingActivitiesTotal,
                    StudentActivitiesTotal = studentActivitiesTotal,
                    PersonalTraitsTotal = personalTraitsTotal,
                    AdministrativeTotal = administrativeTotal,
                    TotalScore = totalScore,
                    MaxScore = maxScore,
                    HodStrengths = evaluation.HodStrengths,
                    HodWeaknesses = evaluation.HodWeaknesses
                };
            }).ToList();

            return result;
        }


        public async Task<bool> UpdateHodEvaluationAsync(int evaluationId, UpdateHodEvaluationDto dto)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                // Remove existing evaluations
                var existingEvaluations = await _db.Hodevaluations
                    .Where(h => h.EvaluationId == evaluationId)
                    .ToListAsync();

                _db.Hodevaluations.RemoveRange(existingEvaluations);

                // Add new evaluations
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

                // Update comments
                var evaluation = await _db.Evaluations.FindAsync(evaluationId);
                if (evaluation != null)
                {
                    evaluation.HodStrengths = dto.HodStrengths;
                    evaluation.HodWeaknesses = dto.HodWeaknesses;
                }

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
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
        private decimal MapScoreToPoints(int scoreValue, string criterionType)
        {
            return criterionType switch
            {
                // Teaching Activities (Criteria 1-5): 5 criteria × 2 points = 10
                "DirectTeaching" => scoreValue switch
                {
                    0 => 0m,    // ضعيف
                    1 => 0.5m,  // مقبول
                    2 => 1.0m,  // جيد
                    3 => 1.5m,  // جيد جداً
                    4 => 2.0m,  // ممتاز
                    _ => 0m
                },

                // Student Activities (Criteria 12-14): 3 criteria sum to 10
                "StudentActivities" => scoreValue switch
                {
                    0 => 0m,     // ضعيف
                    1 => 1.0m,   // مقبول
                    2 => 2.0m,   // جيد
                    3 => 3.0m,   // جيد جداً
                    4 => 3.33m,  // ممتاز
                    _ => 0m
                },

                // Personal Traits (Criteria 15-19): 5 criteria × 2 points = 10
                "PersonalTraits" => scoreValue switch
                {
                    0 => 0m,    // ضعيف
                    1 => 0.5m,  // مقبول
                    2 => 1.0m,  // جيد
                    3 => 1.5m,  // جيد جداً
                    4 => 2.0m,  // ممتاز
                    _ => 0m
                },

                // Administrative (Criterion 20): Direct score 0-10
                "Administrative" or "AdministrativeTotal" => scoreValue,

                _ => 0m
            };
        }
    }
}
