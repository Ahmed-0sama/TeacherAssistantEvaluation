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
        public string TaName { get; set; } = null!;
        public int TaEmployeeId { get; set; }
        public string PeriodName { get; set; } = null!;
        public string StatusName { get; set; } = null!;
        public List<HodEvaluationDto> Evaluations { get; set; } = new();
        public string? HodStrengths { get; set; }
        public string? HodWeaknesses { get; set; }
        public decimal TotalScore { get; set; }
        public decimal MaxScore { get; set; }
    }
}
