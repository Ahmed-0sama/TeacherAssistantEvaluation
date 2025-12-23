using Business_Access.Interfaces;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos.Reminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Access.Services
{
    public class ReminderService : IReminderService
    {
        private readonly SrsDbContext _context;
        public ReminderService(SrsDbContext context)
        {
            _context = context;
        }
        public async Task<bool> SendReminderAsync(SendReminderDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var evaluation = await _context.Evaluations
                    .FirstOrDefaultAsync(s => s.EvaluationId == dto.EvaluationId);

                if (evaluation == null)
                {
                    return false;
                }

                var reminderLog = new ReminderLog
                {
                    EvaluationId = dto.EvaluationId,
                    SentByEmployeeId = dto.SenderEmployeeId,
                    RecievedByEmployeeId = dto.ReciverEmployeeId,
                    RecipientDescription = dto.Message,
                    Timestamp = dto.TimeStamp,
                    IsRead = false,
                };

                _context.ReminderLogs.Add(reminderLog);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return false;
            }
        }
        public async Task<List<ReminderDto>> GetAllReminders(int EmployeeId)
        {
            try
            {
                var reminders = await _context.ReminderLogs
                    .Where(r => r.RecievedByEmployeeId == EmployeeId)
                    .OrderByDescending(r => r.Timestamp)
                    .Select(r => new ReminderDto
                    {
                        reminder = r.LogId,
                        EvaluationId = r.EvaluationId,
                        SenderEmployeeId = r.SentByEmployeeId,
                        ReceiverEmployeeId = r.RecievedByEmployeeId,
                        Message = r.RecipientDescription,
                        TimeStamp = r.Timestamp,
                        IsRead = r.IsRead
                    })
                    .ToListAsync();

                return reminders;
            }
            catch (Exception ex)
            {
                return new List<ReminderDto>();
            }
        }
        public async Task<bool> MarkAsRead(int reminderId)
        {
            try
            {
                var reminder = await _context.ReminderLogs.FirstOrDefaultAsync(s => s.LogId == reminderId);
                if (reminder == null)
                {
                    return false;
                }
                reminder.IsRead = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
