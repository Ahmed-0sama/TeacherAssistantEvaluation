using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Hr
{
    public class EvaluationStatisticsDto
    {
        public int TotalEvaluations { get; set; }
        public int TAPending { get; set; }
        public int HodPending { get; set; }
        public int DeanPending { get; set; }
        public int accepted { get; set; }
    }
}
