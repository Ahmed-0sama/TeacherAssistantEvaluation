using Shared.Dtos.Notifications;
using Shared.Dtos.Reminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Access.Interfaces
{
    public  interface IReminderService
    {
        Task<bool> SendReminderAsync(SendReminderDto dto);
        Task<List<ReminderDto>> GetAllReminders(int EmployeeId);
        Task <bool> MarkAsRead(int notification);

    }
}
