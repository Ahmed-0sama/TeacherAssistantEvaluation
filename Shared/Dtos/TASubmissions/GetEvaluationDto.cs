using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.TASubmissions
{
    public class GetEvaluationDto
    {
        public int EvaluationId { get; set; }
        public int TaEmployeeId { get; set; }
        public int PeriodId { get; set; }
        public string PeriodName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? DateApproved { get; set; }
        public string FinalGrade { get; set; }
        public decimal? StudentSurveyScore { get; set; }

        // Computed
        public bool CanEdit { get; set; }
        public string CurrentStage { get; set; }
        public List<string> PendingActions { get; set; }
    }
}
