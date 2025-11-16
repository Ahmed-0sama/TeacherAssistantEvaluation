using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class ReminderLog
{
    public int LogId { get; set; }

    public int EvaluationId { get; set; }

    public int SentByEmployeeId { get; set; }

    public int RecievedByEmployeeId { get; set; }

    public string RecipientDescription { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public virtual Evaluation Evaluation { get; set; } = null!;
}
