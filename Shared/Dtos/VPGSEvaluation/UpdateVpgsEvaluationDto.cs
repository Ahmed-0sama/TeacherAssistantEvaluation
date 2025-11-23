using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.VPGSEvaluation
{
    public class UpdateVpgsEvaluationDto
    {
        [Required(ErrorMessage = "Scientific score is required")]
        [Range(0, 100, ErrorMessage = "Scientific score must be between 0 and 100")]
        public decimal ScientificScore { get; set; }
    }
}
