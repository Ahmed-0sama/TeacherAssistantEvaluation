using Business_Access.Interfaces;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Dtos.Hr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Access.Services
{
    public class HRService : IHRService
    {
        private readonly SrsDbContext _context;
        private readonly IExternalApiService _externalApiService;
        public HRService(SrsDbContext context, IExternalApiService externalApiService)
        {
            _context = context;
            _externalApiService = externalApiService;
        }
        public async Task<EvaluationStatisticsDto> GetEvaluationStatisticsAsync(int evaluationPeriod, int hrDepartmentId, DateOnly startDate)
        {
            try
            {
                Console.WriteLine($"📡 Loading statistics - Period: {evaluationPeriod}, Department: {hrDepartmentId}");

                // Step 1: Get ALL TAs from external API
                var allTAs = await _externalApiService.GetGTAListAsync(hrDepartmentId, startDate);

                if (allTAs == null || !allTAs.Any())
                {
                    Console.WriteLine("⚠️ No TAs found from external API for statistics");
                    return new EvaluationStatisticsDto();
                }

                var totalTAsCount = allTAs.Count;
                var taIds = allTAs.Select(ta => ta.employeeId).ToHashSet();

                Console.WriteLine($"✅ Total TAs from API: {totalTAsCount}");

                // Step 2: Get evaluation counts in a single query using GroupBy
                var statusCounts = await _context.Evaluations
                    .Where(e => e.PeriodId == evaluationPeriod && taIds.Contains(e.TaEmployeeId))
                    .GroupBy(e => e.StatusId)
                    .Select(g => new { StatusId = g.Key, Count = g.Count() })
                    .ToListAsync();

                // Convert to dictionary for easy lookup
                var statusDict = statusCounts.ToDictionary(x => x.StatusId, x => x.Count);

                // Step 3: Count TAs without evaluations
                var tasWithEvaluations = await _context.Evaluations
                    .Where(e => e.PeriodId == evaluationPeriod && taIds.Contains(e.TaEmployeeId))
                    .Select(e => e.TaEmployeeId)
                    .ToHashSetAsync();

                var tasWithoutEvaluations = taIds.Count(id => !tasWithEvaluations.Contains(id));

                // Step 4: Build statistics using dictionary lookups
                var statistics = new EvaluationStatisticsDto
                {
                    TotalEvaluations = totalTAsCount,

                    // TA Pending: Status 0, 1, 3 + TAs without evaluations
                    TAPending = (statusDict.GetValueOrDefault(0, 0) +
                                statusDict.GetValueOrDefault(1, 0) +
                                statusDict.GetValueOrDefault(3, 0) +
                                tasWithoutEvaluations),

                    // HOD Pending: Status 2, 4, 7
                    HodPending = (statusDict.GetValueOrDefault(2, 0) +
                                 statusDict.GetValueOrDefault(4, 0) +
                                 statusDict.GetValueOrDefault(7, 0)),

                    // Dean Pending: Status 5
                    DeanPending = statusDict.GetValueOrDefault(5, 0),

                    // Accepted: Status 6
                    accepted = statusDict.GetValueOrDefault(6, 0)
                };

                Console.WriteLine($"✅ Statistics calculated:");
                Console.WriteLine($"   - Total: {statistics.TotalEvaluations}");
                Console.WriteLine($"   - TA Pending: {statistics.TAPending}");
                Console.WriteLine($"   - HOD Pending: {statistics.HodPending}");
                Console.WriteLine($"   - Dean Pending: {statistics.DeanPending}");
                Console.WriteLine($"   - Accepted: {statistics.accepted}");

                return statistics;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in GetEvaluationStatisticsAsync: {ex.Message}");
                throw new Exception("An error occurred while retrieving evaluation statistics.", ex);
            }
        }
        public async Task<List<UserDataDto>> GetAllTAsForHRAsync(int periodId, int hrDepartmentId, DateOnly startDate)
        {
            try
            {
                Console.WriteLine($"📡 Loading ALL TAs for HR - Period: {periodId}, Department: {hrDepartmentId}");

                // Step 1: Get TA list from external API
                var taList = await _externalApiService.GetGTAListAsync(hrDepartmentId, startDate);

                if (taList == null || !taList.Any())
                {
                    Console.WriteLine("⚠️ No TAs found from external API");
                    return new List<UserDataDto>();
                }

                Console.WriteLine($"✅ Loaded {taList.Count} TAs from external API");

                // Step 2: Get all evaluations for this period
                var evaluations = await _context.Evaluations
                    .Include(e => e.Period)
                    .Include(e => e.Status)
                    .Include(e => e.Tasubmission)
                    .Where(e => e.PeriodId == periodId)
                    .ToListAsync();

                var evaluationMap = evaluations.ToDictionary(e => e.TaEmployeeId);

                Console.WriteLine($"✅ Found {evaluations.Count} evaluations in database");

                // Step 3: Get HOD evaluations for this period
                var hodEvaluations = await _context.Hodevaluations
                    .Where(h => evaluations.Select(e => e.EvaluationId).Contains(h.EvaluationId) && h.IsActive)
                    .ToListAsync();

                var hodEvaluationMap = hodEvaluations
                    .GroupBy(h => h.EvaluationId)
                    .ToDictionary(g => g.Key, g => g.ToList());

                Console.WriteLine($"✅ Found {hodEvaluationMap.Count} HOD evaluations");

                // Step 4: Enrich TA list with evaluation data
                foreach (var ta in taList)
                {
                    if (evaluationMap.TryGetValue(ta.employeeId, out var evaluation))
                    {
                        // TA has an evaluation
                        ta.EvaluationId = evaluation.EvaluationId;
                        ta.statusid = evaluation.StatusId;
                        ta.HasHodEvaluation = hodEvaluationMap.ContainsKey(evaluation.EvaluationId);
                        ta.TotalScore = evaluation.TotalScore ?? 0;

                        Console.WriteLine($"✅ Enriched TA: {ta.employeeName} - StatusId: {evaluation.StatusId}");
                    }
                    else
                    {
                        ta.EvaluationId = 0;
                        ta.statusid = 0; // No evaluation
                        ta.HasHodEvaluation = false;
                        ta.TotalScore = 0;

                        Console.WriteLine($"ℹ️ TA {ta.employeeName} has no evaluation yet");
                    }

                    ta.EmployeeNumber = ta.employeeId;
                }

                // ✅ Return ALL TAs (HR sees everyone)
                Console.WriteLine($"✅ Total TAs returned for HR: {taList.Count}");

                return taList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in GetAllTAsForHRAsync: {ex.Message}");
                throw new Exception($"Failed to get TAs for HR: {ex.Message}", ex);
            }
        }
    }
}
