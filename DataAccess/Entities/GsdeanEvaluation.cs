using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class GsdeanEvaluation
{
    public int GsevalId { get; set; }
    public int EvaluationPeriodId { get; set; }
    public int? GsdeanEmloyeeId { get; set; }
    public int? TaEmployeeId { get; set; }//it should be int or what?
    public string ProgramName { get; set; } = null!;
    public int CompletedHours { get; set; }
    public decimal Gpa { get; set; }
    public DateOnly ExpectedCompletionDate { get; set; }
    public decimal? ProgressScore { get; set; }
    public string? EvaluationComments { get; set; }
    public bool TopicChosen { get; set; }
    public bool LiteratureReview { get; set; }
    public bool ResearchPlan { get; set; }
    public bool DataCollection { get; set; }
    public bool Writing { get; set; }
    public bool ThesisDefense { get; set; }
    public int StatusId { get; set; }

    public virtual EvaluationPeriod EvaluationPeriod { get; set; } = null!;
    public virtual EvaluationStatus Status { get; set; } = null!;
}
