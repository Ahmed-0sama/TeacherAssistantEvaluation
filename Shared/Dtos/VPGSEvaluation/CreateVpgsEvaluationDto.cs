using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.VPGSEvaluation
{
    public class CreateVpgsEvaluationDto
    {
        [Required(ErrorMessage = "Evaluation ID is required")]
        public int EvaluationId { get; set; }

        [Required(ErrorMessage = "Scientific score is required")]
        [Range(0, 5, ErrorMessage = "Scientific score must be between 0 and 5")]
        public int ScientificScore { get; set; }
    }
}
