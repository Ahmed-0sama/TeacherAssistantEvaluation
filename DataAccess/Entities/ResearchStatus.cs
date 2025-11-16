using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class ResearchStatus
{
    public int StatusId { get; set; }

    public string StatusKey { get; set; } = null!;

    public string StatusName { get; set; } = null!;

    public virtual ICollection<ResearchActivity> ResearchActivities { get; set; } = new List<ResearchActivity>();
}
