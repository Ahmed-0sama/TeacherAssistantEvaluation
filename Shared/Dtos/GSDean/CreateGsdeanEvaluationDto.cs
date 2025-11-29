using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.GSDean
{
    public class CreateGsdeanEvaluationDto
    {
        [Required]
        public int EvaluationPeriodId { get; set; }
        [Required]
        public int TaEmployeeId { get; set; }
        [Required]
        [StringLength(200)]
        public string ProgramName { get; set; }
        [Required]
        public int CompletedHours { get; set; }
        public decimal Gpa { get; set; }
        public decimal? ProgressScore { get; set; }
        [Required]
        public DateOnly ExpectedCompletionDate {  get; set; }
        [StringLength(1000)]
        public string? EvaluationComments { get; set; }
        public bool TopicChosen { get; set; }
        public bool LiteratureReview { get; set; }
        public bool ResearchPlan { get; set; }
        public bool DataCollection { get; set; }
        public bool Writing { get; set; }
        public bool ThesisDefense { get; set; }
    }
}
