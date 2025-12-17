using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.HODEvaluation
{
    public class ReturnToProfessor
    {
        public int EvaluationPeriodId { get; set; }
        public int evaluationId { get; set; }
        public int ProfessorId { get; set; }
        public int TAId { get; set; }
        public string HodComments { get; set; }
        public string TaName { get; set; }
    }
}
