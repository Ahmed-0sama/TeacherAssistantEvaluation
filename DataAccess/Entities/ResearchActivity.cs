using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class ResearchActivity
{
    public int ActivityId { get; set; }

    public int SubmissionId { get; set; }

    public string Title { get; set; } = null!;

    public string Journal { get; set; } = null!;

    public string Location { get; set; } = null!;

    public int PageCount { get; set; }

    public DateOnly ActivityDate { get; set; }

    public int StatusId { get; set; }

    public string? Url { get; set; }

    public virtual ResearchStatus Status { get; set; } = null!;

    public virtual Tasubmission Submission { get; set; } = null!;
}
