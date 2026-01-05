using Business_Access.Interfaces;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos.DeanDto;
using Shared.Dtos.GSDean;
using Shared.Dtos.HODEvaluation;
using Shared.Dtos.Notifications;
using Shared.Dtos.ProfessorEvaluationDto;
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
        private readonly INotification _notificationService;
        private readonly IExternalApiService _externalApiService;
        public DeanServices(SrsDbContext db, INotification notificationService, IExternalApiService externalApiService)
        {
            _db = db;
            _notificationService = notificationService;
            _externalApiService = externalApiService;
        }


        public async Task<DeanEvaluationDetailDto?> GetEvaluationDetailAsync(int evaluationId)
        {
            if (evaluationId <= 0)
                throw new ArgumentException("Invalid evaluation ID", nameof(evaluationId));

            var evaluation = await _db.Evaluations
                .Include(e => e.Status)
                .Include(e => e.Hodevaluations.Where(h => h.IsActive)) 
                    .ThenInclude(h => h.Criterion)
                .Include(e => e.Hodevaluations.Where(h => h.IsActive)) 
                    .ThenInclude(h => h.Rating)
                .Include(e => e.Tasubmission)
                .Include(e => e.Period)
                .FirstOrDefaultAsync(e => e.EvaluationId == evaluationId);

            if (evaluation == null)
                return null;

            var hodEvaluations = evaluation.Hodevaluations?
                .Where(x => x.IsActive)  
                .ToList() ?? new List<Hodevaluation>();

            bool hasDeanEdited = hodEvaluations.Any(h => h.SourceRole == "Dean");

            var teachingActivities = hodEvaluations
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

            // Academic Advising
            var academicAdvisingCount = evaluation.Tasubmission?.AdvisedStudentCount ?? 0;
            var academicAdvising = (decimal)Math.Min(academicAdvisingCount, 5);

            // Use the stored TotalScore from the Evaluation table
            var totalscore = evaluation.TotalScore;

            return new DeanEvaluationDetailDto
            {
                EvaluationId = evaluation.EvaluationId,
                TaEmployeeId = evaluation.TaEmployeeId,
                TaName = $"TA #{evaluation.TaEmployeeId}",
                TaEmail = evaluation.Status?.StatusName ?? "Unknown",
                PeriodName = evaluation.Period?.PeriodName,
                StatusName = evaluation.Status?.StatusName ?? "Unknown",
                StatusId = evaluation.StatusId,

                // Section totals (from ACTIVE rows only)
                TeachingActivitiesTotal = teachingActivities,
                StudentActivitiesTotal = studentActivities,
                PersonalTraitsTotal = personalTraits,
                AdministrativeTotal = administrative,
                ScientificActivityScore = scientificScore,
                //StudentSurveyScore = studentSurvey,
                AcademicAdvisingScore = academicAdvising,
                ProfessorAverageCourseScore = Math.Round(professorAverageScore, 2),

                // Final total
                TotalScore = totalscore ?? 0,
                FinalGrade = GetFinalGrade(totalscore??0),

                // HOD Comments
                HodStrengths = evaluation.HodStrengths,
                HodWeaknesses = evaluation.HodWeaknesses,
                HodReturnComment = evaluation.HodReturnComment,
                DeanReturnComment = evaluation.DeanReturnComment,
                //dean flag for edits
                HasDeanEdited = hasDeanEdited
            };
        }
        public async Task<DeanActionResponseDto> UpdateEvaluationCriteriaAsync(UpdateDeanEvaluationDto dto)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    var evaluation = await _db.Evaluations.FindAsync(dto.EvaluationId);
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

                    if (evaluation.StatusId != 5 && evaluation.StatusId != 7)
                    {
                        await transaction.RollbackAsync();
                        return new DeanActionResponseDto
                        {
                            Success = false,
                            Message = "Evaluation must be in pending or returned status to edit",
                            EvaluationId = dto.EvaluationId
                        };
                    }

                    var activeRows = await _db.Hodevaluations
                        .Where(h => h.EvaluationId == dto.EvaluationId && h.IsActive)
                        .ToListAsync();

                    if (!activeRows.Any())
                    {
                        await transaction.RollbackAsync();
                        return new DeanActionResponseDto
                        {
                            Success = false,
                            Message = "No HOD evaluation found for this evaluation",
                            EvaluationId = dto.EvaluationId
                        };
                    }

                    foreach (var item in dto.CriterionRatings)
                    {
                        var activeRow = activeRows
                     .FirstOrDefault(x => x.CriterionId == item.CriterionId);

                        if (activeRow == null)
                            continue;

                        if (activeRow.RatingId == item.RatingId)
                            continue; 

                        if (activeRow.SourceRole == "Dean")
                        {
                            activeRow.RatingId = item.RatingId;
                            activeRow.CreatedAt = DateTime.UtcNow;
                            activeRow.CreatedByUserId = dto.CreatedByUserId;
                        }
                        else
                        {
                            activeRow.IsActive = false;

                            var newDeanRow = new Hodevaluation
                            {
                                EvaluationId = dto.EvaluationId,
                                CriterionId = item.CriterionId,
                                RatingId = item.RatingId,
                                SourceRole = "Dean",
                                IsActive = true,
                                CreatedAt = DateTime.UtcNow,
                                CreatedByUserId = dto.CreatedByUserId
                            };

                            _db.Hodevaluations.Add(newDeanRow);
                        }
                    }


                    if (dto.TotalScore.HasValue)
                    {
                        evaluation.FinalGrade= GetFinalGrade(dto.TotalScore.Value);
                        evaluation.TotalScore = dto.TotalScore.Value;
                    }

                    if (!string.IsNullOrWhiteSpace(dto.DeanComments))
                    {
                        evaluation.DeanReturnComment = dto.DeanComments;
                    }
                    SendNotificationDto notificationdto = new SendNotificationDto
                    {
                        recipientId = evaluation.TaEmployeeId,
                        message = "Your evaluation has been modified by the Dean."
                    };
                    await _notificationService.SendNotificationAsync(notificationdto);

                    await _db.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new DeanActionResponseDto
                    {
                        Success = true,
                        Message = "Evaluation criteria updated successfully by Dean",
                        EvaluationId = dto.EvaluationId,
                        NewStatus = "Modified by Dean"
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new DeanActionResponseDto
                    {
                        Success = false,
                        Message = $"An error occurred: {ex.Message}",
                        EvaluationId = dto.EvaluationId
                    };
                }
            }
        }
        public string GetFinalGrade(decimal totalscore) {
            return totalscore  switch
            {
                >= 90 => "ممتاز",
                >= 80 => "جيد جداً",
                >= 70 => "جيد",
                >= 60 => "مقبول",
                _ => "ضعيف"
            };
        }
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
                    SendNotificationDto notificationdto = new SendNotificationDto
                    {
                        recipientId = evaluation.TaEmployeeId,
                        message = "Your evaluation has been Accepted by Dean."
                    };
                    await _notificationService.SendNotificationAsync(notificationdto);
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
        public async Task<List<EvaluationApiResponseDto>> GetTAsForDeanAsync(int periodId, int deanDepartmentId, DateOnly startDate)
        {
            try
            {
                Console.WriteLine($" Loading TAs for Dean - Period: {periodId}, Department: {deanDepartmentId}");

                // Step 1: Get TA list from external API
                var taList = await _externalApiService.GetGTAListAsync(deanDepartmentId, startDate);

                if (taList == null || !taList.Any())
                {
                    Console.WriteLine("No TAs found from external API");
                    return new List<EvaluationApiResponseDto>();
                }

                Console.WriteLine($" Loaded {taList.Count} TAs from external API");

                // Step 2: Get all evaluations for this period
                var evaluations = await _db.Evaluations
                    .Include(e => e.Period)
                    .Include(e => e.Status)
                    .Include(e => e.Tasubmission)
                    .Where(e => e.PeriodId == periodId)
                    .ToListAsync();

                var evaluationMap = evaluations.ToDictionary(e => e.TaEmployeeId);

                Console.WriteLine($"✅ Found {evaluations.Count} evaluations in database");

                // Step 3: Build response list with enriched data for ALL GTAs
                var result = new List<EvaluationApiResponseDto>();

                foreach (var ta in taList)
                {
                    if (evaluationMap.TryGetValue(ta.employeeId, out var evaluation))
                    {
                        // TA has an evaluation - show actual status
                        var dto = new EvaluationApiResponseDto
                        {
                            evaluationId = evaluation.EvaluationId,
                            taEmployeeId = ta.employeeId,
                            taName = ta.employeeName,
                            department = ta.Department,
                            periodName = evaluation.Period?.PeriodName ?? "Unknown",
                            statusName = evaluation.Status?.StatusName ?? "Unknown",
                            statusId = evaluation.StatusId
                        };

                        result.Add(dto);
                        Console.WriteLine($"✅ Added TA: {ta.employeeName} - StatusId: {evaluation.StatusId}");
                    }
                    else
                    {
                        //  TA doesn't have an evaluation yet - show as waiting
                        var dto = new EvaluationApiResponseDto
                        {
                            evaluationId = 0, // No evaluation yet
                            taEmployeeId = ta.employeeId,
                            taName = ta.employeeName,
                            department = ta.Department,
                            periodName = "Unknown",
                            statusName = "بانتظار التقييم", // Waiting status
                            statusId = 0 // Status 0 = no evaluation
                        };

                        result.Add(dto);
                        Console.WriteLine($" TA {ta.employeeName} has no evaluation yet - showing as waiting");
                    }
                }

                Console.WriteLine($" Total TAs returned: {result.Count}");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error in GetTAsForDeanAsync: {ex.Message}");
                throw new Exception($"Failed to get TAs for Dean: {ex.Message}", ex);
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
