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
        private const int STATUS_APPROVED_BY_DEAN = 5;
        private const int STATUS_RETURNED_BY_DEAN = 6;

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
                .Include(e => e.Period)
                .Include(e => e.Hodevaluations)
                    .ThenInclude(h => h.Rating)
                .Include(e => e.Hodevaluations)
                    .ThenInclude(h => h.Criterion)
                .Include(e => e.Tasubmission)
                    .ThenInclude(t => t.ResearchActivities)
                        .ThenInclude(r => r.Status)
                .Include(e => e.EvaluationNavigation) // VpgsEvaluation for scientific score
                .FirstOrDefaultAsync(e => e.EvaluationId == evaluationId);

            if (evaluation == null)
                return null;

            var hodEvaluations = evaluation.Hodevaluations?.ToList() ?? new List<Hodevaluation>();
            var hodEval = hodEvaluations.FirstOrDefault();

            // ⭐ ONLY show the HOD's evaluated scores - display them exactly as HOD rated them
            // Get all HOD evaluation scores by category (don't recalculate)
            var directTeachingScores = hodEvaluations
                .Where(h => h.CriterionId >= 1 && h.CriterionId <= 5)
                .Select(h => h.Rating?.ScoreValue ?? 0)
                .ToList();
            var directTeachingScore = directTeachingScores.Any()
                ? directTeachingScores.Average()
                : 0;

            var administrativeScores = hodEvaluations
                .Where(h => h.CriterionId >= 6 && h.CriterionId <= 11)
                .Select(h => h.Rating?.ScoreValue ?? 0)
                .ToList();
            var administrativeScore = administrativeScores.Any()
                ? administrativeScores.Average()
                : 0;

            var studentActivitiesScores = hodEvaluations
                .Where(h => h.CriterionId >= 12 && h.CriterionId <= 14)
                .Select(h => h.Rating?.ScoreValue ?? 0)
                .ToList();
            var studentActivitiesScore = studentActivitiesScores.Any()
                ? studentActivitiesScores.Average()
                : 0;

            var personalTraitsScores = hodEvaluations
                .Where(h => h.CriterionId >= 15 && h.CriterionId <= 19)
                .Select(h => h.Rating?.ScoreValue ?? 0)
                .ToList();
            var personalTraitsScore = personalTraitsScores.Any()
                ? personalTraitsScores.Average()
                : 0;

            // ⭐ Get other scores from submission data
            var studentSurveyScore = evaluation.StudentSurveyScore ?? 0;

            var advisedStudentCount = evaluation.Tasubmission?.AdvisedStudentCount ?? 0;
            var academicAdvisingScore = Math.Min(advisedStudentCount, 10);

            // Get Scientific Activity Score from VpgsEvaluation
            decimal scientificScore = 0;
            if (evaluation.EvaluationNavigation?.ScientificScore > 0)
            {
                scientificScore = evaluation.EvaluationNavigation.ScientificScore;
            }
            else
            {
                var researchActivityCount = evaluation.Tasubmission?.ResearchActivities?.Count ?? 0;
                scientificScore = Math.Min(researchActivityCount * 2, 10);
            }

            // ⭐ Calculate weighted total using HOD's scores
            const decimal directTeachingWeight = 0.50m;
            const decimal administrativeWeight = 0.10m;
            const decimal studentActivitiesWeight = 0.10m;
            const decimal personalTraitsWeight = 0.10m;
            const decimal studentSurveyWeight = 0.05m;
            const decimal academicAdvisingWeight = 0.05m;
            const decimal scientificActivityWeight = 0.10m;

            var weightedTotal =
                (decimal)directTeachingScore * directTeachingWeight +
                (decimal)administrativeScore * administrativeWeight +
                (decimal)studentActivitiesScore * studentActivitiesWeight +
                (decimal)personalTraitsScore * personalTraitsWeight +
                (decimal)studentSurveyScore * studentSurveyWeight +
                (decimal)academicAdvisingScore * academicAdvisingWeight +
                (decimal)scientificScore * scientificActivityWeight;

            var totalScore = Math.Round(weightedTotal, 2);

            return new DeanEvaluationDetailDto
            {
                EvaluationId = evaluation.EvaluationId,

                // TA info
                TaEmployeeId = evaluation.TaEmployeeId,
                TaName = "غير محدد",
                TaEmail = "",

                // Period
                PeriodName = evaluation.Period?.PeriodName ?? "",
                PeriodStartDate = evaluation.Period?.StartDate ?? default,
                PeriodEndDate = evaluation.Period?.EndDate ?? default,

                // Status
                StatusName = evaluation.StatusId,
                StatusId = evaluation.StatusId,

                // HOD Review
                HodStrengths = evaluation.HodStrengths,
                HodWeaknesses = evaluation.HodWeaknesses,
                HodReturnComment = evaluation.HodReturnComment,

                DeanReturnComment = evaluation.DeanReturnComment,
                FinalGrade = evaluation.FinalGrade,
                StudentSurveyScore = evaluation.StudentSurveyScore,
                DateSubmitted = evaluation.DateSubmitted,
                DateApproved = evaluation.DateApproved,

                // ⭐ Component Scores - DIRECTLY FROM HOD EVALUATION
                EducationalActivityScore = Math.Round((decimal)directTeachingScore, 2),
                AdministrativeActivityScore = Math.Round((decimal)administrativeScore, 2),
                StudentActivitiesScore = Math.Round((decimal)studentActivitiesScore, 2),
                PersonalTraitsScore = Math.Round((decimal)personalTraitsScore, 2),
                ScientificActivityScore = Math.Round((decimal)scientificScore, 2),
                AcademicAdvisingScore = Math.Round((decimal)academicAdvisingScore, 2),

                // ⭐ Total Score (0-10 scale)
                TotalScore = totalScore,

                // ⭐ Detailed HOD Evaluations - Show ALL individual HOD ratings
                HodEvaluations = hodEvaluations.Any()
                    ? new HodEvaluationDto
                    {
                        HodevalId = hodEval?.HodevalId ?? 0,
                        EvaluationId = hodEval?.EvaluationId ?? 0,
                        CriterionId = hodEval?.CriterionId ?? 0,
                        CriterionName = hodEval?.Criterion?.CriterionName ?? "",
                        CriterionType = hodEval?.Criterion?.CriterionType ?? "",
                        RatingId = hodEval?.RatingId ?? 0,
                        RatingName = hodEval?.Rating?.RatingName ?? "",
                        ScoreValue = hodEval?.Rating?.ScoreValue ?? 0,
                    }
                    : new HodEvaluationDto(),
                // TA Submission
                TaSubmission = evaluation.Tasubmission != null
                    ? new TASubmissionResponseDto
                    {
                        SubmissionId = evaluation.Tasubmission.SubmissionId,
                        EvaluationId = evaluation.Tasubmission.EvaluationId,

                        ActualTeachingLoad = evaluation.Tasubmission.ActualTeachingLoad,
                        ExpectedTeachingLoad = evaluation.Tasubmission.ExpectedTeachingLoad,

                        HasTechnicalReports = evaluation.Tasubmission.HasTechnicalReports,
                        HasSeminarLectures = evaluation.Tasubmission.HasSeminarLectures,
                        HasAttendingSeminars = evaluation.Tasubmission.HasAttendingSeminars,

                        IsInAcademicAdvisingCommittee = evaluation.Tasubmission.IsInAcademicAdvisingCommittee,
                        IsInSchedulingCommittee = evaluation.Tasubmission.IsInSchedulingCommittee,
                        IsInQualityAssuranceCommittee = evaluation.Tasubmission.IsInQualityAssuranceCommittee,
                        IsInLabEquipmentCommittee = evaluation.Tasubmission.IsInLabEquipmentCommittee,
                        IsInExamOrganizationCommittee = evaluation.Tasubmission.IsInExamOrganizationCommittee,
                        IsInSocialOrSportsCommittee = evaluation.Tasubmission.IsInSocialOrSportsCommittee,

                        ParticipatedInSports = evaluation.Tasubmission.ParticipatedInSports,
                        ParticipatedInSocial = evaluation.Tasubmission.ParticipatedInSocial,
                        ParticipatedInCultural = evaluation.Tasubmission.ParticipatedInCultural,

                        AdvisedStudentCount = evaluation.Tasubmission.AdvisedStudentCount,

                        ResearchActivities = evaluation.Tasubmission.ResearchActivities
                            ?.Select(r => new ResearchActivityResponseDto
                            {
                                ActivityId = r.ActivityId,
                                Title = r.Title,
                                Journal = r.Journal,
                                StatusId = r.StatusId,
                                ActivityDate = r.ActivityDate
                            }).ToList() ?? new List<ResearchActivityResponseDto>(),
                    }
                    : null
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
                    evaluation.StatusId = STATUS_APPROVED_BY_DEAN;
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

                    if (evaluation.StatusId != STATUS_APPROVED_BY_DEAN)
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
                    evaluation.StatusId = STATUS_RETURNED_BY_DEAN;
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
