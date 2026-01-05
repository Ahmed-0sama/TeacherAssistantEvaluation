using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Evaluation
{
    public int EvaluationId { get; set; }

    public int TaEmployeeId { get; set; }

    public int PeriodId { get; set; }

    public int StatusId { get; set; }

    public string? HodStrengths { get; set; }

    public string? HodWeaknesses { get; set; }

    public string? HodReturnComment { get; set; }

    public string? DeanReturnComment { get; set; }

    public string? FinalGrade { get; set; }
    public decimal? TotalScore { get; set; }

    public DateTime? DateSubmitted { get; set; }

    public DateTime? DateApproved { get; set; }
    public virtual AutoCalculatedScore? AutoCalculatedScore { get; set; }
    public virtual VpgsEvaluation EvaluationNavigation { get; set; } = null!;

    public virtual ICollection<Hodevaluation> Hodevaluations { get; set; } = new List<Hodevaluation>();
    public virtual EvaluationPeriod Period { get; set; } = null!;
    public virtual ICollection<ReminderLog> ReminderLogs { get; set; } = new List<ReminderLog>();
    public virtual EvaluationStatus Status { get; set; } = null!;
    public virtual Tasubmission? Tasubmission { get; set; }
}
