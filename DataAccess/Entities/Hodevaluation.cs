using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Hodevaluation
{
    public int HodevalId { get; set; }

    public int EvaluationId { get; set; }
    public string SourceRole { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CreatedByUserId { get; set; }
    public int CriterionId { get; set; }

    public int RatingId { get; set; }

    public virtual HodevaluationCriterion Criterion { get; set; } = null!;

    public virtual Evaluation Evaluation { get; set; } = null!;

    public virtual Rating Rating { get; set; } = null!;

}
