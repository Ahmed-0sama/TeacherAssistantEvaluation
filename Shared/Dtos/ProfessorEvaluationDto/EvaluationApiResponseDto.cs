using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.ProfessorEvaluationDto
{
    public class EvaluationApiResponseDto
    {
        public int evaluationId { get; set; }
        public int taEmployeeId { get; set; }
        public string taName { get; set; } = string.Empty;
        public int periodId { get; set; }
        public string periodName { get; set; } = string.Empty;
        public int statusId { get; set; }
        public string department { get; set; } 
        public int statusName { get; set; }
        public bool canEdit { get; set; }

        public string currentStage { get; set; } = string.Empty;
    }
}
