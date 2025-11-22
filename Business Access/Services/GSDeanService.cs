using Business_Access.Interfaces;
using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos.GSDean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Access.Services
{
    public class GSDeanService:IGSDean
    {
        private readonly SrsDbContext _context;
        public GSDeanService(SrsDbContext context)
        {
            _context = context;
        }
        private static GsdeanEvaluationDto MapToDto(GsdeanEvaluation entity)
        {
            return new GsdeanEvaluationDto
            {
                GsevalId = entity.GsevalId,
                EvaluationId = entity.EvaluationId,
                ProgramName = entity.ProgramName,
                CompletedHours = entity.CompletedHours,
                Gpa = entity.Gpa,
                ExpectedCompletionDate = entity.ExpectedCompletionDate,
                ProgressScore = entity.ProgressScore,
                EvaluationComments = entity.EvaluationComments,
                TopicChosen = entity.TopicChosen,
                LiteratureReview = entity.LiteratureReview,
                ResearchPlan = entity.ResearchPlan,
                DataCollection = entity.DataCollection,
                Writing = entity.Writing,
                ThesisDefense = entity.ThesisDefense
            };
        }
        public async Task<GsdeanEvaluationDto?> GetByIdAsync(int gsevalId)
        {
            if (gsevalId <= 0)
                throw new ArgumentException("Invalid GS Evaluation Id");

            try
            {
                var entity = await _context.GsdeanEvaluations
                    .Include(g => g.Evaluation)
                    .FirstOrDefaultAsync(g => g.GsevalId == gsevalId);

                return entity == null ? null : MapToDto(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving GS Dean evaluation", ex);
            }
        }
        public async Task<IEnumerable<GsdeanEvaluationDto>> GetAllAsync()
        {
            try
            {
                var entities = await _context.GsdeanEvaluations
                    .Include(g => g.Evaluation)
                    .ToListAsync();

                return entities.Select(MapToDto);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving GS Dean evaluations list", ex);
            }
        }
        public async Task<GsdeanEvaluationDto> CreateAsync(CreateGsdeanEvaluationDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var entity = new GsdeanEvaluation
                {
                    EvaluationId = dto.EvaluationId,
                    ProgramName = dto.ProgramName,
                    CompletedHours = dto.CompletedHours,
                    Gpa = dto.Gpa,
                    ExpectedCompletionDate = dto.ExpectedCompletionDate,
                    ProgressScore = dto.ProgressScore,
                    EvaluationComments = dto.EvaluationComments,
                    TopicChosen = dto.TopicChosen,
                    LiteratureReview = dto.LiteratureReview,
                    ResearchPlan = dto.ResearchPlan,
                    DataCollection = dto.DataCollection,
                    Writing = dto.Writing,
                    ThesisDefense = dto.ThesisDefense
                };

                _context.GsdeanEvaluations.Add(entity);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return MapToDto(entity);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error creating GS Dean evaluation", ex);
            }
        }
        public async Task<GsdeanEvaluationDto> UpdateAsync(UpdateGsdeanEvaluationDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var entity = await _context.GsdeanEvaluations.FindAsync(dto.GsevalId);

                if (entity == null)
                    throw new KeyNotFoundException("GS Dean evaluation not found");

                // Update fields
                entity.EvaluationId = dto.EvaluationId;
                entity.ProgramName = dto.ProgramName;
                entity.CompletedHours = dto.CompletedHours;
                entity.Gpa = dto.Gpa;
                entity.ExpectedCompletionDate = dto.ExpectedCompletionDate;
                entity.ProgressScore = dto.ProgressScore;
                entity.EvaluationComments = dto.EvaluationComments;
                entity.TopicChosen = dto.TopicChosen;
                entity.LiteratureReview = dto.LiteratureReview;
                entity.ResearchPlan = dto.ResearchPlan;
                entity.DataCollection = dto.DataCollection;
                entity.Writing = dto.Writing;
                entity.ThesisDefense = dto.ThesisDefense;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return MapToDto(entity);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error updating GS Dean evaluation", ex);
            }
        }
        public async Task<GSDeanTAViewDto> GetByEvaluationIdForTAAsync(int evaluationId)
        {
            if (evaluationId <= 0)
                throw new ArgumentException("Invalid evaluation Id");

            try
            {
                var entity = await _context.GsdeanEvaluations
                    .Include(g => g.Evaluation)
                    .FirstOrDefaultAsync(g => g.EvaluationId == evaluationId);

                if (entity == null)
                    return null;

                return new GSDeanTAViewDto
                {
                    ProgramName = entity.ProgramName,
                    CompletedHours = entity.CompletedHours,
                    Gpa = entity.Gpa,
                    ExpectedCompletionDate = entity.ExpectedCompletionDate,
                    TopicChosen = entity.TopicChosen,
                    LiteratureReview = entity.LiteratureReview,
                    ResearchPlan = entity.ResearchPlan,
                    DataCollection = entity.DataCollection,
                    Writing = entity.Writing,
                    ThesisDefense = entity.ThesisDefense
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching GS dean evaluation for TA", ex);
            }
        }
        public async Task<IEnumerable<GsdeanEvaluationDto>> GetByEvaluationIdAsync(int evaluationId)
        {
            try
            {
                var entities = await _context.GsdeanEvaluations
                    .Include(g => g.Evaluation)
                    .Where(g => g.EvaluationId == evaluationId)
                    .ToListAsync();

                return entities.Select(MapToDto);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching GS dean evaluations by EvaluationId", ex);
            }
        }
    }
}