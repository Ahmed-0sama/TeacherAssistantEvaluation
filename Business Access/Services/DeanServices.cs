using Business_Access.Interfaces;
using DataAccess.Context;
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
        private const int STATUS_PENDING_DEAN_APPROVAL = 3;
        private const int STATUS_APPROVED_BY_DEAN = 4;
        private const int STATUS_RETURNED_BY_DEAN = 5;

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
                .FirstOrDefaultAsync(e => e.EvaluationId == evaluationId);

            if (evaluation == null)
                return null;

            var hodEval = evaluation.Hodevaluations?.FirstOrDefault();

            // ⭐ Calculate scores by category from HOD evaluations
            var hodEvaluations = evaluation.Hodevaluations?.ToList() ?? new List<Hodevaluation>();

            // Calculate Direct Teaching Score (criterion IDs 1-5) - Weight 50%
            var directTeachingScore = hodEvaluations
                .Where(h => h.CriterionId >= 1 && h.CriterionId <= 5)
                .Sum(h => h.Rating?.ScoreValue ?? 0);

            // Calculate Administrative Score (criterion IDs 6-11) - Weight 10%
            var administrativeScore = hodEvaluations
                .Where(h => h.CriterionId >= 6 && h.CriterionId <= 11)
                .Sum(h => h.Rating?.ScoreValue ?? 0);

            // Calculate Student Activities Score (criterion IDs 12-14) - Weight 10%
            var studentActivitiesScore = hodEvaluations
                .Where(h => h.CriterionId >= 12 && h.CriterionId <= 14)
                .Sum(h => h.Rating?.ScoreValue ?? 0);

            // Calculate Personal Traits Score (criterion IDs 15-19) - Weight 10%
            var personalTraitsScore = hodEvaluations
                .Where(h => h.CriterionId >= 15 && h.CriterionId <= 19)
                .Sum(h => h.Rating?.ScoreValue ?? 0);

            // Student Survey Score - Weight 5%
            var studentSurveyScore = evaluation.StudentSurveyScore ?? 0;

            // Academic Advising Score - Weight 5%
            // This comes from TaSubmission.AdvisedStudentCount
            var academicAdvisingScore = 0m; // Will be calculated based on advised students count

            // Scientific Activity Score - Weight 10%
            // This comes from ResearchActivities (published papers, conferences, etc.)
            var scientificScore = 0m; // Will be calculated based on research activities

            // Calculate total score
            var totalScore = directTeachingScore + administrativeScore +
                             studentActivitiesScore + personalTraitsScore +
                             studentSurveyScore + academicAdvisingScore + scientificScore;

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
                StatusName = evaluation.Status?.StatusName ?? "",
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

                // ⭐ NEW: Calculated Scores
                EducationalActivityScore = directTeachingScore,      // Direct Teaching (1-5)
                ScientificActivityScore = scientificScore,           // Research Activities
                AdministrativeActivityScore = administrativeScore,   // Administrative (6-11)
                StudentActivitiesScore = studentActivitiesScore,     // Student Activities (12-14)
                AcademicAdvisingScore = academicAdvisingScore,       // Based on advised students
                PersonalTraitsScore = personalTraitsScore,           // Personal Traits (15-19)
                TotalScore = totalScore,

                // Detailed HOD evaluation (first record)
                HodEvaluations = hodEval != null
                    ? new HodEvaluationDto
                    {
                        HodevalId = hodEval.HodevalId,
                        EvaluationId = hodEval.EvaluationId,
                        CriterionId = hodEval.CriterionId,
                        ScoreValue = hodEval.Rating?.ScoreValue ?? 0,
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
                                StatusId = r.StatusId,  // Fixed: Changed from StatusName to StatusId
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

                    if (evaluation.StatusId != STATUS_PENDING_DEAN_APPROVAL)
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
