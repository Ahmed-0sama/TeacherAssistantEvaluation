using Business_Access.Interfaces;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
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
        public HRService(SrsDbContext context)
        {
            _context = context;
        }
        public async Task<EvaluationStatisticsDto> GetEvaluationStatisticsAsync(int evaluationPeriod)
        {
            try
            {
                var statistics = await _context.Evaluations
                    .Where(e => e.PeriodId == evaluationPeriod)
                    .GroupBy(e => 1)
                    .Select(g => new EvaluationStatisticsDto
                    {
                        TotalEvaluations = g.Count(),
                        TAPending = g.Count(e => e.StatusId == 1 || e.StatusId == 0||e.StatusId==3),
                        HodPending = g.Count(e => e.StatusId == 2||e.StatusId==4||e.StatusId==7),
                        DeanPending = g.Count(e => e.StatusId == 5),
                        accepted = g.Count(e => e.StatusId == 6)
                    })
                    .FirstOrDefaultAsync();
                return statistics;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving evaluation statistics.", ex);
            }
        }
    }
}
