using Business_Access.Interfaces;

using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos.EvaluationPeriod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Access.Services
{
    public class EvaluationPeriodService : IEvaluationPeriod
    {
        private readonly SrsDbContext db;
        public EvaluationPeriodService(SrsDbContext context)
        {
            db = context;
        }
        public async Task<bool> IsCurrentEvaluationPeriodActiveAsync()
        {
            try
            {
                var currentPeriod = await GetCurrentEvaluationPeriodAsync();
                return currentPeriod != null;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new Exception("Error checking if evaluation period is active", ex);
            }
        }

        public async Task<int?> GetCurrentEvaluationPeriodIdAsync()
        {
            try
            {
                var currentPeriod = await GetCurrentEvaluationPeriodAsync();
                return currentPeriod?.PeriodId;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new Exception("Error getting current evaluation period ID", ex);
            }
        }

        public async Task<GetEvaluationPeriodDto> GetCurrentEvaluationPeriodAsync()
        {
            try
            {
                var allPeriods = await db.EvaluationPeriods.ToListAsync();
                var today = DateOnly.FromDateTime(DateTime.Today);

                // Find the period where today falls between start and end date
                var currentPeriod = allPeriods
                    .FirstOrDefault(p => p.StartDate <= today && p.EndDate >= today);

                if (currentPeriod == null)
                    return null;

                GetEvaluationPeriodDto dto = new GetEvaluationPeriodDto
                {
                    PeriodId = currentPeriod.PeriodId,
                    PeriodName = currentPeriod.PeriodName,
                    StartDate = currentPeriod.StartDate,
                    EndDate = currentPeriod.EndDate,
                    IsActive = true
                };
                return dto;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new Exception("Error getting current evaluation period", ex);
            }
        }

        public async Task<int> CreateEvaluationPeriodAsync(CreateEvaluationPeriodDto periodDto)
        {
            // Validate input
            if (periodDto == null)
                throw new ArgumentNullException(nameof(periodDto));

            if (string.IsNullOrWhiteSpace(periodDto.PeriodName))
                throw new ArgumentException("Period name is required", nameof(periodDto.PeriodName));

            if (periodDto.EndDate <= periodDto.StartDate)
                throw new ArgumentException("End date must be after start date");
            using var transaction = await db.Database.BeginTransactionAsync();

            try
            {
                // Check for overlapping periods
                var hasOverlap = await HasOverlappingPeriodAsync(periodDto.StartDate, periodDto.EndDate);
                if (hasOverlap)
                    throw new InvalidOperationException("The new period overlaps with an existing period.");

                // Create new evaluation period
                var period = new EvaluationPeriod
                {
                    PeriodName = periodDto.PeriodName,
                    StartDate = periodDto.StartDate,
                    EndDate = periodDto.EndDate
                };
                db.EvaluationPeriods.Add(period);
                await db.SaveChangesAsync();
                var createdPeriodId = period.PeriodId;

                await transaction.CommitAsync();

                return createdPeriodId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<GetEvaluationPeriodDto> GetEvaluationPeriodByIdAsync(int periodId)
        {
            if (periodId <= 0)
                throw new ArgumentException("Invalid period ID", nameof(periodId));

            try
            {
                var period = await db.EvaluationPeriods.FirstOrDefaultAsync(p=>p.PeriodId==periodId);

                if (period == null)
                    throw new KeyNotFoundException($"Evaluation period with ID {periodId} not found");
                GetEvaluationPeriodDto dto = new GetEvaluationPeriodDto
                {
                    PeriodId = period.PeriodId,
                    PeriodName = period.PeriodName,
                    StartDate = period.StartDate,
                    EndDate = period.EndDate
                };
                dto.IsActive = DateOnly.FromDateTime(DateTime.Now) >= period.StartDate &&
                               DateOnly.FromDateTime(DateTime.Now) <= period.EndDate;
                return dto;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new Exception($"Error getting evaluation period with ID {periodId}", ex);
            }
        }

        public async Task<IEnumerable<GetEvaluationPeriodDto>> GetAllEvaluationPeriodsAsync()
        {
            try
            {
                var periods = await db.EvaluationPeriods.ToListAsync();

                var today = DateOnly.FromDateTime(DateTime.Now);

                // Order by start date descending
                return periods
                            .OrderByDescending(p => p.StartDate)
                            .Select(p => new GetEvaluationPeriodDto
                            {
                                PeriodId = p.PeriodId,
                                PeriodName = p.PeriodName,
                                StartDate = p.StartDate,
                                EndDate = p.EndDate,

                                IsActive = today >= p.StartDate &&
                                           today <= p.EndDate
                            })
                            .ToList();
            }
            catch (Exception ex)
            {

                throw new Exception("Error getting all evaluation periods", ex);
            }
        }

        public async Task UpdateEvaluationPeriodAsync(int periodId, CreateEvaluationPeriodDto periodDto)
        {
            if (periodDto == null)
                throw new ArgumentNullException(nameof(periodDto));

            if (string.IsNullOrWhiteSpace(periodDto.PeriodName))
                throw new ArgumentException("Period name is required", nameof(periodDto.PeriodName));

            if (periodDto.EndDate <= periodDto.StartDate)
                throw new ArgumentException("End date must be after start date");
            using var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                var period = await db.EvaluationPeriods.FirstOrDefaultAsync(p=>p.PeriodId==periodId);
                if (period == null)
                    throw new KeyNotFoundException($"Evaluation period with ID {periodId} not found");

                // Check if period has already started
                var today = DateOnly.FromDateTime(DateTime.Today);
                if (period.StartDate < today)
                {
                    // Only allow updating end date for periods that have started
                    if (periodDto.StartDate != period.StartDate)
                        throw new InvalidOperationException("Cannot modify start date of a period that has already started");
                }

                // Check for overlapping periods (excluding current period)
                var hasOverlap = await HasOverlappingPeriodAsync(
                    periodDto.StartDate,
                    periodDto.EndDate,
                    excludePeriodId: periodId
                );

                if (hasOverlap)
                    throw new InvalidOperationException("The updated dates overlap with an existing period");

                // Update period
                period.PeriodName = periodDto.PeriodName.Trim();
                period.StartDate = periodDto.StartDate;
                period.EndDate = periodDto.EndDate;

                db.EvaluationPeriods.Update(period);
                await db.SaveChangesAsync();
                await db.Database.CommitTransactionAsync();

            }
            catch
            {
                await db.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task CloseEvaluationPeriodAsync(int periodId)
        {
            if (periodId <= 0)
                throw new ArgumentException("Invalid period ID", nameof(periodId));

            try
            {
                using var transaction = await db.Database.BeginTransactionAsync();

                var period = await db.EvaluationPeriods.FirstOrDefaultAsync(p=>p.PeriodId==periodId);
                if (period == null)
                    throw new KeyNotFoundException($"Evaluation period with ID {periodId} not found");

                var today = DateOnly.FromDateTime(DateTime.Today);

                // Check if period is already closed
                if (period.EndDate < today)
                    throw new InvalidOperationException("This evaluation period has already ended");

                // Set end date to today (or yesterday to make it immediately closed)
                period.EndDate = today.AddDays(-1);

                db.EvaluationPeriods.Update(period);
                await db.SaveChangesAsync();

                await db.Database.CommitTransactionAsync();
            }
            catch
            {
                await db.Database.RollbackTransactionAsync();
                throw;
            }
        }

        private async Task<bool> HasOverlappingPeriodAsync(DateOnly startDate, DateOnly endDate, int? excludePeriodId = null)
        {
            var periods = db.EvaluationPeriods.AsNoTracking();


            if (excludePeriodId.HasValue)
            {
                periods = periods.Where(p => p.PeriodId != excludePeriodId.Value);
            }

            return await periods.AnyAsync(p =>
                (startDate >= p.StartDate && startDate <= p.EndDate) ||
                (endDate >= p.StartDate && endDate <= p.EndDate) ||
                (startDate <= p.StartDate && endDate >= p.EndDate)
            );
        }

    }
}
