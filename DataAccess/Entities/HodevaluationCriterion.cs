using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class HodevaluationCriterion
{
    public int CriterionId { get; set; }

    public string CriterionName { get; set; } = null!;

    public string CriterionType { get; set; } = null!;

    public virtual ICollection<Hodevaluation> Hodevaluations { get; set; } = new List<Hodevaluation>();
}
