using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Notifications
{
    public class NotificationDto
    {
        public int NotificationId { get; set; }
        public int SenderId { get; set; }
        public string SenderTitle { get; set; }
        public int ReceiverId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
