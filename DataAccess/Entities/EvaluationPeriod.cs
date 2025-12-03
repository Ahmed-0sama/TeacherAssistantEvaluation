using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class EvaluationPeriod
{
    public int PeriodId { get; set; }

    public string PeriodName { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }
    public virtual ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
    public virtual GsdeanEvaluation? GsdeanEvaluation { get; set; }
    public virtual ICollection<ProfessorCourseEvaluation> ProfessorCourseEvaluations { get; set; } = new List<ProfessorCourseEvaluation>();

}
