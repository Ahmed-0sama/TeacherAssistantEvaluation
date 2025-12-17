using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Notifications
{
    public class SendNotificationDto
    {
        public int EvaluationId{  get; set; }
        public int senderId { get; set; }
        public int recipientId { get; set; }
        public string message { get; set; }
    }
}
