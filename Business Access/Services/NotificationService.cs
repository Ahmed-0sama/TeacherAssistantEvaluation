using Business_Access.Interfaces;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Access.Services
{
    public class NotificationService : INotification
    {
        private readonly SrsDbContext _context;
        public NotificationService(SrsDbContext context)
        {
            _context = context;
        }
        public async Task<bool> SendNotificationAsync(SendNotificationDto dto)
        {
            try
            {
                var notification = new ReminderLog
                {

                    EvaluationId = dto.EvaluationId,

                    SentByEmployeeId = dto.senderId,

                    RecievedByEmployeeId = dto.recipientId,

                    RecipientDescription = dto.message,

                    Timestamp = DateTime.Now,
                };
                _context.ReminderLogs.Add(notification);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<List<NotificationDto>> GetAllNotification(int employeeId)
        {
            try
            {
                var notifications = await _context.ReminderLogs
                    .Where(n => n.RecievedByEmployeeId == employeeId)
                    .OrderByDescending(n => n.Timestamp)
                    .Select(n => new NotificationDto
                    {
                        NotificationId = n.LogId,
                        SenderTitle =n.SenderRole,
                        SenderId = n.SentByEmployeeId,
                        ReceiverId = n.RecievedByEmployeeId,
                        Message = n.RecipientDescription,
                        CreatedAt = n.Timestamp
                    })
                    .ToListAsync();

                return notifications;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load notifications", ex);
            }
        }
    }

}
