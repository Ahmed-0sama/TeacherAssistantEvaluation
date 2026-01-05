using Business_Access.Interfaces;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
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
        private readonly IExternalApiService _externalApiService;

        public GSDeanService(SrsDbContext context, IExternalApiService externalApiService)
        {
            _context = context;
            _externalApiService = externalApiService;
        }
        private static GsdeanEvaluationDto MapToDto(GsdeanEvaluation entity)
        {
            return new GsdeanEvaluationDto
            {
                GsevalId = entity.GsevalId,
                EvaluationPeriodId = entity.EvaluationPeriodId,
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
                    .Include(g => g.EvaluationPeriod)
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
                    .Include(g => g.EvaluationPeriod)
                    .Include(g=>g.Status)
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
                    GsdeanEmloyeeId = dto.GsSupervisorId,
                    EvaluationPeriodId = dto.EvaluationPeriodId,
                    TaEmployeeId= dto.TaEmployeeId,
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
                    ThesisDefense = dto.ThesisDefense,
                    StatusId = 2 // Assuming '1' is the default status for new evaluations
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
        public async Task<GSDeanTAViewDto> GetByEvaluationPeriodAndTAAsync(int evaluationPeriodId, int taEmployeeId)
        {
            if (evaluationPeriodId <= 0)
                throw new ArgumentException("Invalid evaluation period Id");

            if (taEmployeeId<=0)
                throw new ArgumentException("Invalid TA employee Id");

            try
            {
                var entity = await _context.GsdeanEvaluations
                    .Include(g => g.EvaluationPeriod)  
                    .Include(g => g.Status)            
                    .FirstOrDefaultAsync(g => g.EvaluationPeriodId == evaluationPeriodId
                                           && g.TaEmployeeId == taEmployeeId);

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
        public async Task<GsdeanEvaluationDto> GetByEvaluationForGSDeanPeriodAndTAAsync(int evaluationPeriodId, int taEmployeeId)
        {
            if (evaluationPeriodId <= 0)
                throw new ArgumentException("Invalid evaluation period Id");

            if (taEmployeeId<=0)
                throw new ArgumentException("Invalid TA employee Id");

            try
            {
                var entity = await _context.GsdeanEvaluations
                    .Include(g => g.EvaluationPeriod)  
                    .Include(g => g.Status)            
                    .FirstOrDefaultAsync(g => g.EvaluationPeriodId == evaluationPeriodId
                                           && g.TaEmployeeId == taEmployeeId);

                if (entity == null)
                    return null;

               return MapToDto(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching GS dean evaluation for TA", ex);
            }
        }
        public async Task<IEnumerable<GsdeanEvaluationDto>> GetByEvaluationPeriodIdAsync(int evaluationPeriodId)
        {
            try
            {
                var entities = await _context.GsdeanEvaluations
                    .Include(g => g.EvaluationPeriod)  // CHANGED from Evaluation
                    .Include(g => g.Status)            // NEW
                    .Where(g => g.EvaluationPeriodId == evaluationPeriodId)
                    .ToListAsync();

                return entities.Select(MapToDto);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching GS dean evaluations by EvaluationPeriodId", ex);
            }
        }
        public async Task<IEnumerable<GsdeanEvaluationDto>> GetByTAEmployeeIdAsync(int taEmployeeId)
        {
            if (taEmployeeId<=0)
                throw new ArgumentException("Invalid TA employee Id");

            try
            {
                var entities = await _context.GsdeanEvaluations
                    .Include(g => g.EvaluationPeriod)
                    .Include(g => g.Status)
                    .Where(g => g.TaEmployeeId == taEmployeeId)
                    .ToListAsync();

                return entities.Select(MapToDto);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching GS dean evaluations by TA employee Id", ex);
            }
        }
        public async Task<List<UserDataDto>> GetGTAListWithEvaluationsAsync(
        int supervisorId,
        int evaluationPeriodId,
         DateOnly startDate)
        {
            try
            {
                // Step 1: Get GTA list from external API
                var gtaList = await _externalApiService.GetGTAListAsync(supervisorId, startDate);

                if (gtaList == null || !gtaList.Any())
                {
                    Console.WriteLine(" No GTAs found");
                    return new List<UserDataDto>();
                }

                Console.WriteLine($"Loaded {gtaList.Count} GTAs from external API");

                // Step 2: Get all evaluations for this period
                var evaluations = await _context.GsdeanEvaluations
                    .Where(e => e.EvaluationPeriodId == evaluationPeriodId)
                    .Include(e => e.Status)
                    .Include(e => e.EvaluationPeriod)
                    .ToListAsync();

                // Step 3: Enrich GTA data with evaluation status
                foreach (var gta in gtaList)
                {
                    var evaluation = evaluations.FirstOrDefault(e => e.TaEmployeeId == gta.employeeId);

                    if (evaluation != null)
                    {
                        gta.statusid = 2; // Has evaluation - read only
                        gta.EvaluationId = evaluation.GsevalId;
                    }
                    else
                    {
                        gta.statusid = 1; // No evaluation yet - can evaluate
                        gta.EvaluationId = 0;
                    }
                }

                return gtaList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error in GetGTAListWithEvaluationsAsync: {ex.Message}");
                throw new Exception($"Failed to get GTA list with evaluations: {ex.Message}", ex);
            }
        }
    }
}