using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.DeanDto
{
    public class ReturnEvaluationDto
    {
        [Required(ErrorMessage = "Evaluation ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Evaluation ID must be greater than 0")]
        public int EvaluationId { get; set; }

        [Required(ErrorMessage = "Return comment is required")]
        [MaxLength(1000, ErrorMessage = "Return comment cannot exceed 1000 characters")]
        public string DeanReturnComment { get; set; }
    }
}
