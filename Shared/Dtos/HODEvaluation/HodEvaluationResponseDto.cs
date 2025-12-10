using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.HODEvaluation
{
    public class HodEvaluationResponseDto
    {
        public int EvaluationId { get; set; }
        public string TaName { get; set; }
        public int TaEmployeeId { get; set; }
        public int StatusId { get; set; }
        public List<HodEvaluationItemDto> Evaluations { get; set; }
        public decimal TeachingActivitiesTotal { get; set; }
        public decimal StudentActivitiesTotal { get; set; }
        public decimal PersonalTraitsTotal { get; set; }
        public decimal AdministrativeTotal { get; set; }
        public decimal TotalScore { get; set; }
        public decimal MaxScore { get; set; }
        public string HodStrengths { get; set; }
        public string HodWeaknesses { get; set; }
        public string? DeanReturnComments { get; set; }
    }
}
