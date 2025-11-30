using Business_Access.Interfaces;
using DataAccess.Context;
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
                .Include(e => e.Tasubmission)
                    .ThenInclude(t => t.ResearchActivities)
                .Include(e => e.Tasubmission)
                .Include(e => e.TaEmployeeId) // TA info
                .FirstOrDefaultAsync(e => e.EvaluationId == evaluationId);

            if (evaluation == null)
                return null;

            var hodEval = evaluation.Hodevaluations?.FirstOrDefault();

            return new DeanEvaluationDetailDto
            {
                EvaluationId = evaluation.EvaluationId,

                // TA info
                TaEmployeeId = evaluation.TaEmployeeId,

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

                // Detailed HOD evaluation (first record)
                HodEvaluations = hodEval != null
                    ? new HodEvaluationDto
                    {
                        HodevalId = hodEval.HodevalId,
                        EvaluationId = hodEval.EvaluationId,
                        CriterionId = hodEval.CriterionId,
                        ScoreValue = hodEval.Rating.ScoreValue,
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
                                StatusName = r.Status.StatusName,
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
