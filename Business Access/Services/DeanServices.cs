using Business_Access.Interfaces;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos.DeanDto;
using Shared.Dtos.HODEvaluation;
using Shared.Dtos.TASubmissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Access.Services
{
    public class DeanServices : IDean
    {

        private readonly SrsDbContext _db;
        public DeanServices(SrsDbContext db)
        {
            _db = db;
        }


        public async Task<DeanEvaluationDetailDto?> GetEvaluationDetailAsync(int evaluationId)
        {
            if (evaluationId <= 0)
                throw new ArgumentException("Invalid evaluation ID", nameof(evaluationId));

            var evaluation = await _db.Evaluations
                .Include(e => e.Status)
                .Include(e => e.Hodevaluations)
                    .ThenInclude(h => h.Criterion)
                .Include(e => e.Hodevaluations)
                    .ThenInclude(h => h.Rating)
                .Include(e => e.Tasubmission)
                .Include(e => e.Period)
                .FirstOrDefaultAsync(e => e.EvaluationId == evaluationId);

            if (evaluation == null)
                return null;

            // HOD Data (only needed to calculate totals)
            var hodEvaluations = evaluation.Hodevaluations?.ToList() ?? new List<Hodevaluation>();

            // Aggregate Section Totals - ensure decimal type
            var teachingActivities = (decimal)hodEvaluations
                .Where(x => x.Criterion?.CriterionType == "DirectTeaching")
                .Sum(x => x.Rating != null
                       ? MapScoreToPoints(x.Rating.ScoreValue, "DirectTeaching")
                       : 0);

            var studentActivities = (decimal)hodEvaluations
                .Where(x => x.Criterion?.CriterionType == "StudentActivities")
                .Sum(x => x.Rating != null
                       ? MapScoreToPoints(x.Rating.ScoreValue, "StudentActivities")
                       : 0);

            var personalTraits = (decimal)hodEvaluations
                .Where(x => x.Criterion?.CriterionType == "PersonalTraits")
                .Sum(x => x.Rating != null
                       ? MapScoreToPoints(x.Rating.ScoreValue, "PersonalTraits")
                       : 0);

            var administrative = (decimal)hodEvaluations
                .Where(x => x.Criterion?.CriterionType == "Administrative" ||
                            x.Criterion?.CriterionType == "AdministrativeTotal")
                .Sum(x => x.Rating != null
                       ? MapScoreToPoints(x.Rating.ScoreValue, "Administrative")
                       : 0);

            // Professor Course Evaluations - Query through EvaluationPeriod
            var professorEvaluations = await _db.ProfessorCourseEvaluations
                .Where(p => p.TaEmployeeId == evaluation.TaEmployeeId &&
                            p.EvaluationPeriodId == evaluation.PeriodId)
                .ToListAsync();

            var professorAverageScore = professorEvaluations.Any(p => p.TotalScore.HasValue)
                ? (decimal)professorEvaluations
                    .Where(p => p.TotalScore.HasValue)
                    .Average(p => p.TotalScore!.Value)
                : 0m;

            // Scientific Score - Sum of VPGS and GS Dean scores
            var vpgsScore = await _db.VpgsEvaluations
                .Where(v => v.EvaluationId == evaluationId)
                .Select(v => v.ScientificScore)
                .FirstOrDefaultAsync();

            var gsDeanScore = await _db.GsdeanEvaluations
                .Where(g => g.TaEmployeeId == evaluation.TaEmployeeId &&
                            g.EvaluationPeriodId == evaluation.PeriodId)
                .Select(g => g.ProgressScore ?? 0)
                .FirstOrDefaultAsync();

            var scientificScore = vpgsScore + gsDeanScore;

            // Student Survey
            var studentSurvey = (decimal)(evaluation.StudentSurveyScore ?? 0);

            // Academic Advising
            var academicAdvisingCount = evaluation.Tasubmission?.AdvisedStudentCount ?? 0;
            var academicAdvising = (decimal)Math.Min(academicAdvisingCount, 5);

            // Calculate total
            var totalScore =
                teachingActivities +
                studentActivities +
                personalTraits +
                administrative +
                scientificScore +
                studentSurvey +
                professorAverageScore +
                academicAdvising;

            // Grade simple mapping
            var finalGrade = totalScore switch
            {
                >= 90 => "ممتاز",
                >= 80 => "جيد جداً",
                >= 70 => "جيد",
                >= 60 => "مقبول",
                _ => "ضعيف"
            };

            return new DeanEvaluationDetailDto
            {
                EvaluationId = evaluation.EvaluationId,
                TaEmployeeId = evaluation.TaEmployeeId,
                TaName = $"TA #{evaluation.TaEmployeeId}",
                TaEmail = evaluation.Status?.StatusName ?? "Unknown", // TODO: Get actual department/email
                PeriodName = evaluation.Period?.PeriodName,
                StatusName = evaluation.Status?.StatusName ?? "Unknown",
                StatusId = evaluation.StatusId,

                // Section totals
                TeachingActivitiesTotal = teachingActivities,
                StudentActivitiesTotal = studentActivities,
                PersonalTraitsTotal = personalTraits,
                AdministrativeTotal = administrative,
                ScientificActivityScore = scientificScore,
                StudentSurveyScore = studentSurvey,
                AcademicAdvisingScore = academicAdvising,
                ProfessorAverageCourseScore = Math.Round(professorAverageScore, 2),

                // Final total
                TotalScore = (int)Math.Round(totalScore),
                FinalGrade = finalGrade,

                // HOD Comments
                HodStrengths = evaluation.HodStrengths,
                HodWeaknesses = evaluation.HodWeaknesses,
                HodReturnComment = evaluation.HodReturnComment,
                DeanReturnComment = evaluation.DeanReturnComment
            };
        }
        // ⭐ Add the MapScoreToPoints method (SAME AS HOD SERVICE)
        private decimal MapScoreToPoints(int scoreValue, string criterionType)
        {
            return criterionType switch
            {
                "DirectTeaching" => scoreValue switch      // Criteria 1-5 (max 10 points total)
                {
                    0 => 0m,
                    1 => 0.5m,
                    2 => 1m,
                    3 => 1.5m,
                    4 => 2m,
                    _ => 0m
                },
                "StudentActivities" => scoreValue switch   // Criteria 12-14 (max 10 points total)
                {
                    0 => 0m,
                    1 => 1m,
                    2 => 2m,
                    3 => 3m,
                    4 => 3.33m,
                    _ => 0m
                },
                "PersonalTraits" => scoreValue switch      // Criteria 15-19 (max 10 points total)
                {
                    0 => 0m,
                    1 => 0.5m,
                    2 => 1m,
                    3 => 1.5m,
                    4 => 2m,
                    _ => 0m
                },
                "Administrative" or "AdministrativeTotal" => scoreValue switch  // Criteria 6-11 + 20 (max 10 points)
                {
                    0 => 0m,
                    1 => 1m,
                    2 => 2m,
                    3 => 3m,
                    4 => 4m,
                    5 => 5m,
                    6 => 6m,
                    7 => 7m,
                    8 => 8m,
                    9 => 9m,
                    10 => 10m,
                    _ => 0m
                },
                _ => 0m
            };
        }

        public async Task<DeanActionResponseDto> AcceptEvaluationAsync(AcceptEvaluationDto dto)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    var evaluation = await _db.Evaluations
                        .Include(e => e.Status)
                        .FirstOrDefaultAsync(e => e.EvaluationId == dto.EvaluationId);

                    if (evaluation == null)
                    {
                        await transaction.RollbackAsync();
                        return new DeanActionResponseDto
                        {
                            Success = false,
                            Message = "Evaluation not found",
                            EvaluationId = dto.EvaluationId
                        };
                    }

                    // Update evaluation status
                    evaluation.StatusId = 6;
                    evaluation.DateApproved = DateTime.UtcNow;
                    await _db.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return new DeanActionResponseDto
                    {
                        Success = true,
                        Message = "Evaluation approved successfully",
                        EvaluationId = dto.EvaluationId,
                        NewStatus = "Approved by Dean"
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    // Log exception here
                    return new DeanActionResponseDto
                    {
                        Success = false,
                        Message = $"An error occurred: {ex.Message}",
                        EvaluationId = dto.EvaluationId
                    };
                }
            }
        }

        public async Task<DeanActionResponseDto> ReturnEvaluationAsync(ReturnEvaluationDto dto)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    var evaluation = await _db.Evaluations
                        .Include(e => e.Status)
                        .FirstOrDefaultAsync(e => e.EvaluationId == dto.EvaluationId);

                    if (evaluation == null)
                    {
                        await transaction.RollbackAsync();
                        return new DeanActionResponseDto
                        {
                            Success = false,
                            Message = "Evaluation not found",
                            EvaluationId = dto.EvaluationId
                        };
                    }

                    if (evaluation.StatusId != 5)
                    {
                        await transaction.RollbackAsync();
                        return new DeanActionResponseDto
                        {
                            Success = false,
                            Message = "Evaluation is not in pending dean approval status",
                            EvaluationId = dto.EvaluationId,
                            NewStatus = evaluation.Status.StatusName
                        };
                    }

                    if (string.IsNullOrWhiteSpace(dto.DeanReturnComment))
                    {
                        await transaction.RollbackAsync();
                        return new DeanActionResponseDto
                        {
                            Success = false,
                            Message = "Return comment is required",
                            EvaluationId = dto.EvaluationId
                        };
                    }

                    // Update evaluation status and add comment
                    evaluation.StatusId = 7;
                    evaluation.DeanReturnComment = dto.DeanReturnComment;
                    await _db.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return new DeanActionResponseDto
                    {
                        Success = true,
                        Message = "Evaluation returned to HOD successfully",
                        EvaluationId = dto.EvaluationId,
                        NewStatus = "Returned by Dean"
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    // Log exception here
                    return new DeanActionResponseDto
                    {
                        Success = false,
                        Message = $"An error occurred: {ex.Message}",
                        EvaluationId = dto.EvaluationId
                    };
                }

            }
        }
    }
}
