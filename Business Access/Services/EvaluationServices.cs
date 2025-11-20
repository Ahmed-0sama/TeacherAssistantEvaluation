using Business_Access.Interfaces;
using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos.TASubmissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Access.Services
{
    public class EvaluationServices:IEvaluation
    {
        private readonly SrsDbContext db;
        public EvaluationServices(SrsDbContext db)
        {
            this.db = db;
        }
        
        public async Task<int> CreateEvaluationAsync(int taEmployeeId, int periodId)
        {
            try
            {
                var period = await db.EvaluationPeriods.FirstOrDefaultAsync(p => p.PeriodId == periodId);
                if (period == null)
                {
                    throw new Exception($"Evaluation Period Not Found");
                }
                var existingEvaluation = await db.Evaluations
                    .FirstOrDefaultAsync(e => e.TaEmployeeId == taEmployeeId && e.PeriodId == periodId);
                if (existingEvaluation != null)
                {
                    throw new Exception("Evaluation already exists for the given TA and period.");
                }
                var evaluation = new Evaluation
                {
                    TaEmployeeId = taEmployeeId,
                    PeriodId = periodId,
                    StatusId = 1, // Assuming 1 is the default status for a new evaluation
                    DateSubmitted = null,
                    DateApproved = null
                };
                db.Evaluations.Add(evaluation);
                await db.SaveChangesAsync();
                return evaluation.EvaluationId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating evaluation: {ex.Message}");
            }

        }
        public async Task<int> SubmitTAFilesAsync(int evaluationId, CreateTASubmissionDto submissionDto)
        {
            var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                var evaluation = await db.Evaluations
                    .Include(e => e.Tasubmission)
                    .FirstOrDefaultAsync(e => e.EvaluationId == evaluationId);
                if (evaluation == null)
                {
                    throw new KeyNotFoundException($"Evaluation with ID {evaluationId} not found");

                }
                if (evaluation.Tasubmission != null)
                    throw new InvalidOperationException("TA has already submitted files for this evaluation");

                var submission = new Tasubmission
                {
                    EvaluationId = evaluationId,
                    ActualTeachingLoad = submissionDto.ActualTeachingLoad,
                    ExpectedTeachingLoad = submissionDto.ExpectedTeachingLoad,
                    HasTechnicalReports = submissionDto.HasTechnicalReports,
                    HasSeminarLectures = submissionDto.HasSeminarLectures,
                    HasAttendingSeminars = submissionDto.HasAttendingSeminars,
                    AdvisedStudentCount = submissionDto.AdvisedStudentCount
                };

                db.Tasubmissions.Add(submission);
                await db.SaveChangesAsync();

                // Add research activities if any
                if (submissionDto.ResearchActivities?.Any() == true)
                {
                    foreach (var activityDto in submissionDto.ResearchActivities)
                    {
                        var activity = new ResearchActivity
                        {
                            SubmissionId = submission.SubmissionId,
                            Title = activityDto.Title,
                            Journal = activityDto.Journal,
                            Location = activityDto.Location,
                            PageCount = activityDto.PageCount,
                            ActivityDate = activityDto.ActivityDate,
                            StatusId = activityDto.StatusId,
                            Url = activityDto.Url
                        };
                        db.ResearchActivities.Add(activity);
                    }
                    await db.SaveChangesAsync();
                }

                // Update evaluation status to "Submitted"
                evaluation.StatusId = 1; // Submitted status - adjust based on your status IDs
                evaluation.DateSubmitted = DateTime.Now;
                db.Evaluations.Update(evaluation);
                await db.SaveChangesAsync();

                await transaction.CommitAsync();
                return submission.SubmissionId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task UpdateTASubmissionAsync(int evaluationid, UpdateTASubmissionsDto submissionDto)
        {
            using var transaction = await db.Database.BeginTransactionAsync();

            try
            {
                var submission = await db.Tasubmissions
                    .Include(s => s.ResearchActivities)
                    .FirstOrDefaultAsync(s => s.EvaluationId == evaluationid);

                if (submission == null)
                    throw new KeyNotFoundException($"TA submission with ID {evaluationid} not found");

                var evaluation = await db.Evaluations
                    .FirstOrDefaultAsync(e => e.EvaluationId == submission.EvaluationId);

                if (evaluation == null)
                    throw new KeyNotFoundException("Associated evaluation not found");

                submission.ActualTeachingLoad = submissionDto.ActualTeachingLoad;
                submission.ExpectedTeachingLoad = submissionDto.ExpectedTeachingLoad;
                submission.HasTechnicalReports = submissionDto.HasTechnicalReports;
                submission.HasSeminarLectures = submissionDto.HasSeminarLectures;
                submission.HasAttendingSeminars = submissionDto.HasAttendingSeminars;
                submission.AdvisedStudentCount = submissionDto.AdvisedStudentCount;

                // Update added boolean fields
                submission.IsInAcademicAdvisingCommittee = submissionDto.IsInAcademicAdvisingCommittee;
                submission.IsInSchedulingCommittee = submissionDto.IsInSchedulingCommittee;
                submission.IsInQualityAssuranceCommittee = submissionDto.IsInQualityAssuranceCommittee;
                submission.IsInLabEquipmentCommittee = submissionDto.IsInLabEquipmentCommittee;
                submission.IsInExamOrganizationCommittee = submissionDto.IsInExamOrganizationCommittee;
                submission.IsInSocialOrSportsCommittee = submissionDto.IsInSocialOrSportsCommittee;

                submission.ParticipatedInSports = submissionDto.ParticipatedInSports;
                submission.ParticipatedInSocial = submissionDto.ParticipatedInSocial;
                submission.ParticipatedInCultural = submissionDto.ParticipatedInCultural;
                // Remove existing research activities and add new ones
                if (submission.ResearchActivities.Any())
                {
                    db.ResearchActivities.RemoveRange(submission.ResearchActivities);
                }

                // Add updated research activities
                if (submissionDto.ResearchActivities?.Any() == true)
                {
                    foreach (var activityDto in submissionDto.ResearchActivities)
                    {
                        var activity = new ResearchActivity
                        {
                            SubmissionId = submission.SubmissionId,
                            Title = activityDto.Title,
                            Journal = activityDto.Journal,
                            Location = activityDto.Location,
                            PageCount = activityDto.PageCount,
                            ActivityDate = activityDto.ActivityDate,
                            StatusId = activityDto.StatusId,
                            Url = activityDto.Url
                        };
                        db.ResearchActivities.Add(activity);
                    }
                }

                db.Tasubmissions.Update(submission);
                await db.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<TASubmissionResponseDto> GetTASubmissionAsync(int evaluationId)
        {
            try
            {
                var submission = await db.Tasubmissions
                    .Include(s => s.ResearchActivities)
                        .ThenInclude(ra => ra.Status)
                    .Include(s => s.Evaluation)
                    .FirstOrDefaultAsync(s => s.EvaluationId == evaluationId);

                if (submission == null)
                    throw new KeyNotFoundException($"TA submission not found for evaluation {evaluationId}");

                return new TASubmissionResponseDto
                {
                    SubmissionId = submission.SubmissionId,
                    EvaluationId = submission.EvaluationId,
                    ActualTeachingLoad = submission.ActualTeachingLoad,
                    ExpectedTeachingLoad = submission.ExpectedTeachingLoad,
                    HasTechnicalReports = submission.HasTechnicalReports,
                    HasSeminarLectures = submission.HasSeminarLectures,
                    HasAttendingSeminars = submission.HasAttendingSeminars,
                    AdvisedStudentCount = submission.AdvisedStudentCount,
                    ParticipatedInCultural = submission.ParticipatedInCultural,
                    ParticipatedInSocial = submission.ParticipatedInSocial,
                    ParticipatedInSports = submission.ParticipatedInSports,
                    IsInLabEquipmentCommittee = submission.IsInLabEquipmentCommittee,
                    IsInAcademicAdvisingCommittee = submission.IsInAcademicAdvisingCommittee,
                    IsInExamOrganizationCommittee = submission.IsInExamOrganizationCommittee,
                    CommitteeParticipation = new CommiteParticipationDto
                    {
                        IsInAcademicAdvisingCommittee = submission.IsInAcademicAdvisingCommittee,
                        IsInSchedulingCommittee = submission.IsInSchedulingCommittee,
                        IsInQualityAssuranceCommittee = submission.IsInQualityAssuranceCommittee,
                        IsInLabEquipmentCommittee = submission.IsInLabEquipmentCommittee,
                        IsInExamOrganizationCommittee = submission.IsInExamOrganizationCommittee,
                        IsInSocialOrSportsCommittee = submission.IsInSocialOrSportsCommittee,
                        ParticipatedInSports = submission.ParticipatedInSports,
                        ParticipatedInSocial = submission.ParticipatedInSocial,
                        ParticipatedInCultural = submission.ParticipatedInCultural
                    },
                    ResearchActivities = submission.ResearchActivities.Select(ra => new ResearchActivityResponseDto
                    {
                        ActivityId = ra.ActivityId,
                        Title = ra.Title,
                        Journal = ra.Journal,
                        Location = ra.Location,
                        PageCount = ra.PageCount,
                        ActivityDate = ra.ActivityDate,
                        StatusName = ra.Status?.StatusName ?? "",
                        Url = ra.Url
                    }).ToList(),
                    SubmittedDate = submission.Evaluation.DateSubmitted ?? DateTime.MinValue
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting TA submission for evaluation {evaluationId}", ex);
            }
        }

        public async Task<GetEvaluationDto> GetEvaluationByIdAsync(int evaluationId)
        {
            try
            {
                var evaluation = await db.Evaluations
                    .Include(e => e.Period)
                    .Include(e => e.Status)
                    .Include(e => e.Tasubmission)
                    .FirstOrDefaultAsync(e => e.EvaluationId == evaluationId);

                if (evaluation == null)
                    throw new KeyNotFoundException($"Evaluation with ID {evaluationId} not found");

                return MapToGetEvaluationDto(evaluation);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting evaluation with ID {evaluationId}", ex);
            }
        }

        public async Task<GetEvaluationDto?> GetTAEvaluationForCurrentPeriodAsync(int taEmployeeId)
        {
            try
            {
                // Get current active period
                var today = DateOnly.FromDateTime(DateTime.Today);
                var currentPeriod = await db.EvaluationPeriods
                    .FirstOrDefaultAsync(p => p.StartDate <= today && p.EndDate >= today);

                if (currentPeriod == null)
                    return null;

                var evaluation = await db.Evaluations
                    .Include(e => e.Period)
                    .Include(e => e.Status)
                    .Include(e => e.Tasubmission)
                    .FirstOrDefaultAsync(e => e.TaEmployeeId == taEmployeeId && e.PeriodId == currentPeriod.PeriodId);

                return evaluation != null ? MapToGetEvaluationDto(evaluation) : null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting evaluation for TA {taEmployeeId} in current period", ex);
            }
        }

        public async Task<IEnumerable<GetEvaluationDto>> GetEvaluationsForTAAsync(int taEmployeeId)
        {
            try
            {
                var evaluations = await db.Evaluations
                    .Include(e => e.Period)
                    .Include(e => e.Status)
                    .Include(e => e.Tasubmission)
                    .Where(e => e.TaEmployeeId == taEmployeeId)
                    .OrderByDescending(e => e.Period.StartDate)
                    .ToListAsync();

                return evaluations.Select(MapToGetEvaluationDto);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting evaluations for TA {taEmployeeId}", ex);
            }
        }

        public async Task<IEnumerable<GetEvaluationDto>> GetEvaluationsByStatusAsync(int statusId)
        {
            try
            {
                var evaluations = await db.Evaluations
                    .Include(e => e.Period)
                    .Include(e => e.Status)
                    .Include(e => e.Tasubmission)
                    .Where(e => e.StatusId == statusId)
                    .OrderBy(e => e.TaEmployeeId)
                    .ToListAsync();

                return evaluations.Select(MapToGetEvaluationDto);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting evaluations by status {statusId}", ex);
            }
        }

        public async Task<IEnumerable<GetEvaluationDto>> GetEvaluationsByPeriodAsync(int periodId)
        {
            try
            {
                var evaluations = await db.Evaluations
                    .Include(e => e.Period)
                    .Include(e => e.Status)
                    .Include(e => e.Tasubmission)
                    .Where(e => e.PeriodId == periodId)
                    .OrderBy(e => e.TaEmployeeId)
                    .ToListAsync();

                return evaluations.Select(MapToGetEvaluationDto);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting evaluations for period {periodId}", ex);
            }
        }
        public async Task<int?> CanTAEditEvaluationAsync(int taEmployeeId)
        {
            try
            {
                var evaluation = await db.Evaluations
                    .FirstOrDefaultAsync(e => e.TaEmployeeId == taEmployeeId);

                if (evaluation == null)
                    return null;
                if (evaluation.StatusId == 1)
                {
                    return evaluation.EvaluationId;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking if TA can edit evaluation {taEmployeeId}", ex);
            }
        }

        private static GetEvaluationDto MapToGetEvaluationDto(Evaluation evaluation)
        {
            return new GetEvaluationDto
            {
                EvaluationId = evaluation.EvaluationId,
                TaEmployeeId = evaluation.TaEmployeeId,
                PeriodId = evaluation.PeriodId,
                PeriodName = evaluation.Period?.PeriodName ?? "",
                StatusId = evaluation.StatusId,
                StatusName = evaluation.Status?.StatusName ?? "",
                DateSubmitted = evaluation.DateSubmitted,
                DateApproved = evaluation.DateApproved,
                FinalGrade = evaluation.FinalGrade ?? "",
                StudentSurveyScore = evaluation.StudentSurveyScore,
                CanEdit = evaluation.StatusId == 1 || evaluation.StatusId == 6, // Draft or Returned
                CurrentStage = GetCurrentStage(evaluation.StatusId),
                PendingActions = GetPendingActions(evaluation.StatusId)
            };
        }

        private static string GetCurrentStage(int statusId)
        {
            return statusId switch
            {
                1 => "Draft - TA Working",
                2 => "Submitted - Pending HOD Review",
                3 => "HOD Reviewed - Pending Professor Evaluation",
                4 => "Professor Evaluated - Pending Dean Approval",
                5 => "Dean Approved - Pending VPGS Review",
                6 => "Returned for Revision",
                7 => "Completed",
                _ => "Unknown Status"
            };
        }
        private static List<string> GetPendingActions(int statusId)
        {
            return statusId switch
            {
                1 => new List<string> { "Complete submission", "Submit for review" },
                2 => new List<string> { "Waiting for HOD review" },
                3 => new List<string> { "Waiting for Professor evaluation" },
                4 => new List<string> { "Waiting for Dean approval" },
                5 => new List<string> { "Waiting for VPGS review" },
                6 => new List<string> { "Address feedback", "Resubmit" },
                7 => new List<string> { "Process completed" },
                _ => new List<string> { "Unknown" }
            };
        }

    }

}