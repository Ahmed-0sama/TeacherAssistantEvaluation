using Business_Access.Interfaces;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos.DeanDto;
using Shared.Dtos.HODEvaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Access.Services
{
    public class DeanServices:IDean
    {
        private readonly SrsDbContext _db;
        public DeanServices(SrsDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<DeanEvaluationListDto>> GetAllEvaluationsAsync(
             int? statusId = null,
             int? periodId = null)
        {
            var query = _db.Evaluations
                .Include(e => e.Status)
                .Include(e => e.Period)
                .AsQueryable();

            if (statusId.HasValue)
                query = query.Where(e => e.StatusId == statusId.Value);

            if (periodId.HasValue)
                query = query.Where(e => e.PeriodId == periodId.Value);

            return await query
                .Select(e => new DeanEvaluationListDto
                {
                    EvaluationId = e.EvaluationId,
                    TaEmployeeId = e.TaEmployeeId,
                    PeriodName = e.Period.PeriodName,
                    StatusName = e.Status.StatusName,
                    FinalGrade = e.FinalGrade,
                    StudentSurveyScore = e.StudentSurveyScore,
                    DateSubmitted = e.DateSubmitted,
                    HodStrengths = e.HodStrengths,
                    HodWeaknesses = e.HodWeaknesses
                })
                .OrderByDescending(e => e.DateSubmitted)
                .ToListAsync();
        }

        public async Task<DeanEvaluationDetailDto?> GetEvaluationDetailAsync(int evaluationId)
        {
            var evaluation = await _db.Evaluations
                .Include(e => e.Status)
                .Include(e => e.Period)
                .Include(e => e.Hodevaluations)
                .Include(e => e.Tasubmission)
                .FirstOrDefaultAsync(e => e.EvaluationId == evaluationId);

            if (evaluation == null)
                return null;

            return new DeanEvaluationDetailDto
            {
                EvaluationId = evaluation.EvaluationId,
                PeriodName = evaluation.Period.PeriodName,
                PeriodStartDate = evaluation.Period.StartDate,
                PeriodEndDate = evaluation.Period.EndDate,
                StatusName = evaluation.Status.StatusName,
                StatusId = evaluation.StatusId,
                HodStrengths = evaluation.HodStrengths,
                HodWeaknesses = evaluation.HodWeaknesses,
                HodReturnComment = evaluation.HodReturnComment,
                DeanReturnComment = evaluation.DeanReturnComment,
                FinalGrade = evaluation.FinalGrade,
                StudentSurveyScore = evaluation.StudentSurveyScore,
                DateSubmitted = evaluation.DateSubmitted,
                DateApproved = evaluation.DateApproved,
                HodEvaluations = evaluation.Hodevaluations.Select(h => new HodEvaluationDto
                {
                    HodevaluationId = h.HodevaluationId,
                    HodName = h.Hod.FirstName + " " + h.Hod.LastName,
                    Comments = h.Comments,
                    Rating = h.Rating,
                    DateEvaluated = h.DateEvaluated
                }).ToList(),
                TaSubmission = evaluation.Tasubmission != null ? new TaSubmissionDto
                {
                    TasubmissionId = evaluation.Tasubmission.TasubmissionId,
                    SelfAssessment = evaluation.Tasubmission.SelfAssessment,
                    Achievements = evaluation.Tasubmission.Achievements,
                    ChallengesFaced = evaluation.Tasubmission.ChallengesFaced,
                    DateSubmitted = evaluation.Tasubmission.DateSubmitted
                } : null
            };
        }

        public async Task<DeanActionResponseDto> AcceptEvaluationAsync(AcceptEvaluationDto dto)
        {
            var evaluation = await _context.Evaluations
                .Include(e => e.Status)
                .FirstOrDefaultAsync(e => e.EvaluationId == dto.EvaluationId);

            if (evaluation == null)
            {
                return new DeanActionResponseDto
                {
                    Success = false,
                    Message = "Evaluation not found",
                    EvaluationId = dto.EvaluationId
                };
            }

            if (evaluation.StatusId != STATUS_PENDING_DEAN_APPROVAL)
            {
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

            if (!string.IsNullOrWhiteSpace(dto.FinalGrade))
            {
                evaluation.FinalGrade = dto.FinalGrade;
            }

            await _context.SaveChangesAsync();

            return new DeanActionResponseDto
            {
                Success = true,
                Message = "Evaluation approved successfully",
                EvaluationId = dto.EvaluationId,
                NewStatus = "Approved by Dean"
            };
        }

        public async Task<DeanActionResponseDto> ReturnEvaluationAsync(ReturnEvaluationDto dto)
        {
            var evaluation = await _context.Evaluations
                .Include(e => e.Status)
                .FirstOrDefaultAsync(e => e.EvaluationId == dto.EvaluationId);

            if (evaluation == null)
            {
                return new DeanActionResponseDto
                {
                    Success = false,
                    Message = "Evaluation not found",
                    EvaluationId = dto.EvaluationId
                };
            }

            if (evaluation.StatusId != STATUS_PENDING_DEAN_APPROVAL)
            {
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

            await _context.SaveChangesAsync();

            return new DeanActionResponseDto
            {
                Success = true,
                Message = "Evaluation returned to HOD successfully",
                EvaluationId = dto.EvaluationId,
                NewStatus = "Returned by Dean"
            };
        }
    }
}
