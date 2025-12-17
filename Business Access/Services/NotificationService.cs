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
                var notification = new Notification
                {
                    EmployeeId = dto.recipientId,
                    Message= dto.message,
                    Timestamp = DateTime.Now,
                    IsRead = false
                };
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //it will return the last 10 notification for employee
        public async Task<List<NotificationDto>> GetAllNotification(int employeeId)
        {
            try
            {
                var notifications = await _context.Notifications
                    .Where(n => n.EmployeeId == employeeId)
                    .OrderByDescending(n => n.Timestamp)
                    .Take(10)
                    .Select(n => new NotificationDto
                    {
                        NotificationId = n.NotificationId,
                        ReceiverId = n.EmployeeId,
                        Message = n.Message,
                        CreatedAt = n.Timestamp,
                        markedAsRead = n.IsRead
                    })
                    .ToListAsync();

                return notifications;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load notifications", ex);
            }
        }
        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(notificationId);
                if (notification == null)
                {
                    return false;
                }
                notification.IsRead = true;
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