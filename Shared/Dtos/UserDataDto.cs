using Shared.Dtos.HODEvaluation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class UserDataDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Requird")]
            public string Name { get; set; } = string.Empty;

            [Required(ErrorMessage = "Requird")]
            public int EmployeeNumber { get; set; }

            [Required(ErrorMessage = "Requird")]
            public string Qualification { get; set; } = string.Empty;

            [Required(ErrorMessage = "Requird")]
            public string Department { get; set; } = string.Empty;
            [Required(ErrorMessage = "Requird")]
            public string College { get; set; } = string.Empty;
            public int statusid { get; set; }
             public int EvaluationId { get; set; }
             public bool? HasVpgsEvaluation { get; set; }
        public bool HasHodEvaluation { get; set; }
        public int HodEvaluationStatus { get; set; }  // New: to track HOD status
        public HodEvaluationResponseDto? HodEvaluationData { get; set; }  // New: full HOD data
    }
}
