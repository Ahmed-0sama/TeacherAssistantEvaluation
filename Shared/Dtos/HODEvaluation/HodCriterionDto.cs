using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.HODEvaluation
{
    public class HodCriterionDto
    {
        public int CriterionId { get; set; }
        public string CriterionName { get; set; } = null!;
        public string CriterionType { get; set; } = null!;
    }
}
