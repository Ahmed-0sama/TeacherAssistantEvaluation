using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int EmployeeId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public bool IsRead { get; set; }
}
