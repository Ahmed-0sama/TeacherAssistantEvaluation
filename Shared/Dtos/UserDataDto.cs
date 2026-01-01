using Shared.Dtos.HODEvaluation;
using Shared.Dtos.ProfessorEvaluationDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class UserDataDto
    {
            public int employeeId { get; set; }
            [Required(ErrorMessage = "Requird")]
            public string employeeName { get; set; } = string.Empty;

            [Required(ErrorMessage = "Requird")]
            public int EmployeeNumber { get; set; }

            [Required(ErrorMessage = "Requird")]
            public string jobTitle { get; set; } = string.Empty;

            [Required(ErrorMessage = "Requird")]
            public string Department { get; set; } = string.Empty;
            [Required(ErrorMessage = "Requird")]
            public string college { get; set; } = string.Empty;
            public int statusid { get; set; }
             public int EvaluationId { get; set; }
             public bool HasVpgsEvaluation { get; set; }
        public bool HasSubmitted { get; set; }
        [JsonPropertyName("evaluation")]
        public ProfessorEvaluationResponseDto? Evaluation { get; set; }
        [JsonPropertyName("isEvaluated")]
        public bool IsEvaluated { get; set; }

        [JsonPropertyName("evaluationStatus")]
        public string EvaluationStatus { get; set; } = "Not Graded";
        public bool HasHodEvaluation { get; set; }
        public int HodEvaluationStatus { get; set; }

        public HodEvaluationResponseDto? HodEvaluationData { get; set; }  // New: full HOD data
    }
}
