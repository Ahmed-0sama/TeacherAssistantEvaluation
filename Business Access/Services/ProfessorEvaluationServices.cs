using Business_Access.Interfaces;
using DataAccess.Context;
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
                var evaluation = await _db.Evaluations.Include(s => s.Status)
                    .FirstOrDefaultAsync(e => e.EvaluationId == evaluationDto.EvaluationId);

                if (evaluation == null)
                    throw new KeyNotFoundException($"Evaluation with ID {evaluationDto.EvaluationId} not found");

                // Check if professor already evaluated this TA for this course
                var existingEvaluation = await _db.ProfessorCourseEvaluations
                    .FirstOrDefaultAsync(pe =>
                        pe.EvaluationId == evaluationDto.EvaluationId &&
                        pe.ProfessorEmployeeId == evaluationDto.ProfessorEmployeeId &&
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
                    EvaluationId = evaluationDto.EvaluationId,
                    ProfessorEmployeeId = evaluationDto.ProfessorEmployeeId,
                    CourseCode = evaluationDto.CourseCode.Trim(),
                    CourseName = evaluationDto.CourseName.Trim(),
                    OfficeHoursScore = evaluationDto.OfficeHoursScore,
                    AttendanceScore = evaluationDto.AttendanceScore,
                    PerformanceScore = evaluationDto.PerformanceScore,
                    TotalScore = totalScore,
                    Comments = evaluationDto.Comments,
                    IsReturned = false,
                    HodReturnComment = null
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
                .Include(pe => pe.Evaluation)
                    .ThenInclude(e => e.Period)
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
                .Include(pe => pe.Evaluation)
                    .ThenInclude(e => e.Period)
                .Include(pe => pe.Evaluation)
                    .ThenInclude(e => e.Status)
                .Where(pe => pe.ProfessorEmployeeId == professorEmployeeId)
                .OrderByDescending(pe => pe.Evaluation.Period.StartDate)
                .ThenBy(pe => pe.CourseCode)
                .ToListAsync();

            return evaluations.Select(pe => MapToResponseDto(pe)).ToList();
        }
        public async Task UpdateProfessorEvaluationAsync(int evaluationid, UpdateProfessorEvaluationDto evaluationDto)
        {
            if (evaluationDto == null)
                throw new ArgumentNullException(nameof(evaluationDto));

            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var profEvaluation = await _db.ProfessorCourseEvaluations
                    .Include(pe => pe.Evaluation)
                    .FirstOrDefaultAsync(pe => pe.EvaluationId == evaluationid);

                if (profEvaluation == null)
                    throw new KeyNotFoundException($"evaluation with ID {evaluationid} not found");

                // Check if evaluation can still be edited
                if (profEvaluation.Evaluation.StatusId > 6) // After HOD review
                    throw new InvalidOperationException("Cannot update evaluation after HOD review stage");

                // Update fields
                profEvaluation.CourseCode = evaluationDto.CourseCode.Trim();
                profEvaluation.CourseName = evaluationDto.CourseName.Trim();
                profEvaluation.OfficeHoursScore = evaluationDto.OfficeHoursScore;
                profEvaluation.AttendanceScore = evaluationDto.AttendanceScore;
                profEvaluation.PerformanceScore = evaluationDto.PerformanceScore;
                profEvaluation.TotalScore = evaluationDto.OfficeHoursScore +
                                           evaluationDto.AttendanceScore +
                                           evaluationDto.PerformanceScore;
                profEvaluation.Comments = evaluationDto.Comments?.Trim();

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
        public async Task<List<ProfessorCourseEvaluation>> GetByEvaluationIdAsync(int evaluationId)
        {
            // Validate input
            if (evaluationId <= 0)
                throw new ArgumentException("Invalid evaluationId. It must be a positive number.", nameof(evaluationId));

                var evaluations = await _db.ProfessorCourseEvaluations
                    .Where(p => p.EvaluationId == evaluationId)
                    .ToListAsync();
                if (evaluations == null || !evaluations.Any())
                {
                    return new List<ProfessorCourseEvaluation>();
                }

                return evaluations;
        }
        private ProfessorEvaluationResponseDto MapToResponseDto(ProfessorCourseEvaluation profEvaluation)
        {
            var professorName = $"Professor {profEvaluation.ProfessorEmployeeId}"; // Placeholder


            return new ProfessorEvaluationResponseDto
            {
                ProfEvalId = profEvaluation.ProfEvalId,
                EvaluationId = profEvaluation.EvaluationId,
                ProfessorEmployeeId = profEvaluation.ProfessorEmployeeId,
                ProfessorName = professorName,
                CourseCode = profEvaluation.CourseCode,
                CourseName = profEvaluation.CourseName,
                OfficeHoursScore = profEvaluation.OfficeHoursScore,
                AttendanceScore = profEvaluation.AttendanceScore,
                PerformanceScore = profEvaluation.PerformanceScore,
                TotalScore = profEvaluation.TotalScore ?? 0,
                Comments = profEvaluation.Comments,
                IsReturned = profEvaluation.IsReturned,
                HodReturnComment = profEvaluation.HodReturnComment,
                IsSubmitted = true,
                SubmittedDate = DateTime.Now.Date
            };
        }
    }
}