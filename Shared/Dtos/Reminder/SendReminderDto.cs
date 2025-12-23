using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Reminder
{
    public class SendReminderDto
    {
        public int EvaluationId { get; set; }
        public int SenderEmployeeId { get; set; }
        public string Message { get; set; }
        public int ReciverEmployeeId{ get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
