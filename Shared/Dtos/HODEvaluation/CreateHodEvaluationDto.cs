using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.HODEvaluation
{
    public class CreateHodEvaluationDto
    {
        public int EvaluationId { get; set; }
        public List<CriterionRatingDto> CriterionRatings { get; set; } = new();
        public string? HodStrengths { get; set; }
        public string? HodWeaknesses { get; set; }
    }
}
