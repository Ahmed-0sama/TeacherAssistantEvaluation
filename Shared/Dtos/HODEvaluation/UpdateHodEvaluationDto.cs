using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.HODEvaluation
{
    public class UpdateHodEvaluationDto
    {
        public List<CriterionRatingDto> CriterionRatings { get; set; }
        public string HodStrengths { get; set; }
        public string HodWeaknesses { get; set; }
    }
}
