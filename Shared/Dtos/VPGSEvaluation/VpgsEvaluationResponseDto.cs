using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.VPGSEvaluation
{
    public class VpgsEvaluationResponseDto
    {
        public int VpgsevalId { get; set; }
        public int EvaluationId { get; set; }
        public decimal ScientificScore { get; set; }
        public int TAEmployeeId { get; set; }
        public string PeriodName { get; set; }
        public string StatusName { get; set; }
    }
}
