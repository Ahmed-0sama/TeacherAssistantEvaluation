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
        public string PeriodName { get; set; }
        public string StatusName { get; set; }
        public int StatusId { get; set; }  // ADD THIS
        public List<HodEvaluationDto> Evaluations { get; set; }
        public string HodStrengths { get; set; }
        public string HodWeaknesses { get; set; }
        public int TotalScore { get; set; }
        public int MaxScore { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int AdministrativeCommitteeScore { get; set; }
    }
}
