using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Reminder
{
    public class ReminderDto
    {
       public int reminder { get; set; }
        public int EvaluationId { get; set; }
        public int SenderEmployeeId { get; set; }
        public int  ReceiverEmployeeId { get; set; }
        public string Message { get; set; }
        public DateTime  TimeStamp { get; set; }
    }
}
