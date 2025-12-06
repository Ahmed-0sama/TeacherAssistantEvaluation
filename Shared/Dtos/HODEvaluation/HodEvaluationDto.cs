using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.HODEvaluation
{
    public class HodEvaluationDto
    {
        public int HodevalId { get; set; }
        public int EvaluationId { get; set; }
        public int CriterionId { get; set; }
        public string CriterionName { get; set; } = null!;
        public string CriterionType { get; set; } = null!;
        public int RatingId { get; set; }
        public string RatingName { get; set; } = null!;
        public int ScoreValue { get; set; }
        public int statusid { get; set; }
    }
}
