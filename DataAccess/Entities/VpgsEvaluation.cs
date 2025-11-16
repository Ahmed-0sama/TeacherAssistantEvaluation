using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class VpgsEvaluation
{
    public int VpgsevalId { get; set; }

    public int EvaluationId { get; set; }

    public decimal ScientificScore { get; set; }

    public virtual Evaluation Evaluation { get; set; } = null!;
}
