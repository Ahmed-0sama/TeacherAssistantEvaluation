using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.DeanDto
{
    public class DeanEvaluationListDto
    {
        public int EvaluationId { get; set; }
        public string TaName { get; set; }
        public int TaEmployeeId { get; set; }
        public string PeriodName { get; set; }
        public string StatusName { get; set; }
        public string? FinalGrade { get; set; }
        public decimal? StudentSurveyScore { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public string? HodStrengths { get; set; }
        public string? HodWeaknesses { get; set; }
    }
}
