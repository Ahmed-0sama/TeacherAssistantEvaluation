using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.DeanDto
{
    public class AcceptEvaluationDto
    {
        [Required(ErrorMessage = "Evaluation ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Evaluation ID must be greater than 0")]
        public int EvaluationId { get; set; }

        public DateOnly DateApproved { get; set; }
    }
}
