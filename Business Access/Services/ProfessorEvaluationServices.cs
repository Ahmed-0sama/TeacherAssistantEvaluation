using Business_Access.Interfaces;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos.ProfessorEvaluationDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Access.Services
{
    public class ProfessorEvaluationServices : IProfessorEvaluation
    {
        private readonly SrsDbContext _db;
        public ProfessorEvaluationServices(SrsDbContext db)
        {
            _db = db;
        }

        public async Task<int> CreateProfessorEvaluationAsync(CreateProfessorEvaluationDto evaluationDto)
        {
            if (evaluationDto == null)
                throw new ArgumentNullException(nameof(evaluationDto));
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var evaluationPeriod = await _db.EvaluationPeriods
                    .FirstOrDefaultAsync(ep => ep.PeriodId == evaluationDto.EvaluationPeriodId);

                if (evaluationPeriod == null)
                    throw new KeyNotFoundException($"Evaluation with ID {evaluationDto.EvaluationPeriodId} not found");

                // Check if professor already evaluated this TA for this course
                var existingEvaluation = await _db.ProfessorCourseEvaluations
                    .FirstOrDefaultAsync(pe =>
                        pe.EvaluationPeriodId == evaluationDto.EvaluationPeriodId &&
                        pe.TaEmployeeId == evaluationDto.TaEmployeeId &&
                        pe.CourseCode == evaluationDto.CourseCode.Trim());

                if (existingEvaluation != null)
                    throw new InvalidOperationException("You have already evaluated this TA for this course");

                // Calculate total score
                int totalScore = evaluationDto.OfficeHoursScore +
                                evaluationDto.AttendanceScore +
                                evaluationDto.PerformanceScore;

                // Create professor evaluation
                var profEvaluation = new ProfessorCourseEvaluation
                {
                    EvaluationPeriodId = evaluationDto.EvaluationPeriodId,
                    TaEmployeeId=evaluationDto.TaEmployeeId,
                    ProfessorEmployeeId = evaluationDto.ProfessorEmployeeId,
                    CourseCode = evaluationDto.CourseCode.Trim(),
                    SemesterName= evaluationDto.SemesterName,
                    CourseName = evaluationDto.CourseName.Trim(),
                    OfficeHoursScore = evaluationDto.OfficeHoursScore,
                    AttendanceScore = evaluationDto.AttendanceScore,
                    PerformanceScore = evaluationDto.PerformanceScore,
                    TotalScore = totalScore,
                    Comments = evaluationDto.Comments,
                    IsReturned = false,
                    HodReturnComment = null,
                    //to be added to get the status of the professor added to the ta
                    StatusId = 2// Submitted
                };

                _db.ProfessorCourseEvaluations.Add(profEvaluation);
                await _db.SaveChangesAsync();

                await transaction.CommitAsync();

                return profEvaluation.ProfEvalId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ProfessorEvaluationResponseDto> GetProfessorEvaluationByIdAsync(int profEvalId)
        {
            if(profEvalId ==0)
            {
                throw new ArgumentException("Invalid professor evaluation ID", nameof(profEvalId));
            }
            var profEvaluation = await _db.ProfessorCourseEvaluations
                .Include(pe => pe.EvaluationPeriod)  
                .Include(pe => pe.Status)            
                .FirstOrDefaultAsync(pe => pe.ProfEvalId == profEvalId);

            if (profEvaluation == null)
                throw new KeyNotFoundException($"Professor evaluation with ID {profEvalId} not found");

            return MapToResponseDto(profEvaluation);
        }
        public async Task<IEnumerable<ProfessorEvaluationResponseDto>> GetEvaluationsByProfessorAsync(int professorEmployeeId)
        {
            if (professorEmployeeId == 0)
                throw new ArgumentException("Invalid professor employee ID", nameof(professorEmployeeId));

            var evaluations = await _db.ProfessorCourseEvaluations
                .Include(pe => pe.EvaluationPeriod) 
                .Include(pe => pe.Status)            
                .Where(pe => pe.ProfessorEmployeeId == professorEmployeeId)
                .OrderByDescending(pe => pe.EvaluationPeriod.StartDate) 
                .ThenBy(pe => pe.CourseCode)
                .ToListAsync();

            return evaluations.Select(pe => MapToResponseDto(pe)).ToList();
        }
        public async Task UpdateProfessorEvaluationAsync(int profEvalId, UpdateProfessorEvaluationDto evaluationDto)
        {
            if (evaluationDto == null)
                throw new ArgumentNullException(nameof(evaluationDto));

            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var profEvaluation = await _db.ProfessorCourseEvaluations
                     .Include(pe => pe.EvaluationPeriod)  // CHANGED from Evaluation
                     .Include(pe => pe.Status)            // NEW
                     .FirstOrDefaultAsync(pe => pe.ProfEvalId == profEvalId);  // CHANGED

                if (profEvaluation == null)
                    throw new KeyNotFoundException($"Professor evaluation with ID {profEvalId} not found");

                // CHANGED: Check status directly on the evaluation
                if (profEvaluation.StatusId > 6) // After HOD review
                    throw new InvalidOperationException("Cannot update evaluation after HOD review stage");

                // Update fields
                profEvaluation.StatusId = 2; // Reset status to Submitted upon update
                profEvaluation.CourseCode = evaluationDto.CourseCode.Trim();
                profEvaluation.CourseName = evaluationDto.CourseName.Trim();
                profEvaluation.SemesterName = evaluationDto.SemesterName;
                profEvaluation.OfficeHoursScore = evaluationDto.OfficeHoursScore;
                profEvaluation.AttendanceScore = evaluationDto.AttendanceScore;
                profEvaluation.PerformanceScore = evaluationDto.PerformanceScore;
                profEvaluation.TotalScore = evaluationDto.OfficeHoursScore +
                                           evaluationDto.AttendanceScore +
                                           evaluationDto.PerformanceScore;
                profEvaluation.Comments = evaluationDto.Comments?.Trim();
               // profEvaluation.StatusId = evaluationDto.StatusId;  // NEW - allow status update

                _db.ProfessorCourseEvaluations.Update(profEvaluation);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<List<ProfessorCourseEvaluation>> GetByEvaluationIdAsync(int evaluationPeriodId)
        {
            // Validate input
            if (evaluationPeriodId <= 0)
                throw new ArgumentException("Invalid evaluationPeriodId. It must be a positive number.", nameof(evaluationPeriodId));

            var evaluations = await _db.ProfessorCourseEvaluations
                .Include(p => p.EvaluationPeriod)  
                .Include(p => p.Status)            
                .Where(p => p.EvaluationPeriodId == evaluationPeriodId)
                .ToListAsync();

            if (evaluations == null || !evaluations.Any())
            {
                return new List<ProfessorCourseEvaluation>();
            }

            return evaluations;
        }
        public async Task<List<ProfessorCourseEvaluation>> GetByTAEmployeeIdAsync(int taEmployeeId)
        {
            if (taEmployeeId<=0)
                throw new ArgumentException("Invalid TA employee ID", nameof(taEmployeeId));

            var evaluations = await _db.ProfessorCourseEvaluations
                .Include(p => p.EvaluationPeriod)
                .Include(p => p.Status)
                .Where(p => p.TaEmployeeId == taEmployeeId)
                .ToListAsync();

            return evaluations ?? new List<ProfessorCourseEvaluation>();
        }

        public async Task <List<ProfessorEvaluationResponseDto>> GetByPeriodAndTAAsync(int evaluationPeriodId, int taEmployeeId)
        {
            if (evaluationPeriodId <= 0)
                throw new ArgumentException("Invalid evaluationPeriodId", nameof(evaluationPeriodId));

            var evaluation = await _db.ProfessorCourseEvaluations
                .Include(p => p.EvaluationPeriod)
                .Include(p => p.Status)
                .Where(p => p.EvaluationPeriodId == evaluationPeriodId && p.TaEmployeeId == taEmployeeId).ToListAsync();

            return evaluation.Select(MapToResponseDto).ToList();
        }

        private ProfessorEvaluationResponseDto MapToResponseDto(ProfessorCourseEvaluation profEvaluation)
        {
            var professorName = $"Professor {profEvaluation.ProfessorEmployeeId}"; // Placeholder

            return new ProfessorEvaluationResponseDto
            {
                ProfEvalId = profEvaluation.ProfEvalId,
                EvaluationPeriodId = profEvaluation.EvaluationPeriodId,  
                TaEmployeeId = profEvaluation.TaEmployeeId,              // NEW
                ProfessorEmployeeId = profEvaluation.ProfessorEmployeeId,
                ProfessorName = professorName,
                SemesterName=profEvaluation.SemesterName,
                CourseCode = profEvaluation.CourseCode,
                CourseName = profEvaluation.CourseName,
                OfficeHoursScore = profEvaluation.OfficeHoursScore,
                AttendanceScore = profEvaluation.AttendanceScore,
                PerformanceScore = profEvaluation.PerformanceScore,
                TotalScore = profEvaluation.TotalScore ?? 0,
                Comments = profEvaluation.Comments,
                IsReturned = profEvaluation.IsReturned,
                HodReturnComment = profEvaluation.HodReturnComment,
                StatusId = profEvaluation.StatusId,                     
                PeriodName = profEvaluation.EvaluationPeriod?.PeriodName, 
                StatusName = profEvaluation.Status?.StatusName,          
                IsSubmitted = true,
                SubmittedDate = DateTime.Now.Date
            };
        }
    }
}