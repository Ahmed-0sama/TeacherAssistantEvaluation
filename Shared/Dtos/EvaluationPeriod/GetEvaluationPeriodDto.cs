using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.EvaluationPeriod
{
    public class GetEvaluationPeriodDto
    {
        public int PeriodId { get; set; }
        public string PeriodName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        // Computed properties for UI
        public bool IsActive { get; set; }
    }
}
