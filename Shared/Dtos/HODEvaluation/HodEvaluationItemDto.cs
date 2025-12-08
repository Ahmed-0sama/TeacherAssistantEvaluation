using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.HODEvaluation
{
    public class HodEvaluationItemDto
    {
        public int CriterionId { get; set; }
        public string CriterionName { get; set; }
        public string CriterionType { get; set; }
        public int RatingId { get; set; }
        public string RatingName { get; set; }
        public int ScoreValue { get; set; }
        public decimal ActualPoints { get; set; } //
    }
}
