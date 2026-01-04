using Shared.Dtos.HODEvaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.DeanDto
{
    public class UpdateDeanEvaluationDto
    {
        public int EvaluationId { get; set; }
        public List<CriterionRatingDto> CriterionRatings { get; set; } = new();
        public decimal? TotalScore { get; set; }
        public string? DeanComments { get; set; }
        public int CreatedByUserId { get; set; }  // Dean's user ID
    }
}
