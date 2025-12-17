using Shared.Dtos.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Access.Interfaces
{
    public interface INotification
    {
        Task<bool> SendNotificationAsync(SendNotificationDto dto);
        Task<List<NotificationDto>> GetAllNotification(int EmployeeId);
    }
}
