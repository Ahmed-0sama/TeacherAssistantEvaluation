using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class EvaluationStatus
{
    public int StatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public string? StatusDescription { get; set; }
    public virtual ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
    public virtual ICollection<GsdeanEvaluation> GsdeanEvaluations { get; set; } = new List<GsdeanEvaluation>();
    public virtual ICollection<ProfessorCourseEvaluation> ProfessorCourseEvaluations { get; set; } = new List<ProfessorCourseEvaluation>();
    public virtual ICollection<VpgsEvaluation> VpgsEvaluations { get; set; } = new List<VpgsEvaluation>();

}
