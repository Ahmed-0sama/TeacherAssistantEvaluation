using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.GSDean
{
    public class GSDeanTAViewDto
    {
        public string ProgramName { get; set; } = null!;
        public int CompletedHours { get; set; }
        public decimal Gpa { get; set; }
        public DateOnly ExpectedCompletionDate { get; set; }
        public bool TopicChosen { get; set; }
        public bool LiteratureReview { get; set; }
        public bool ResearchPlan { get; set; }
        public bool DataCollection { get; set; }
        public bool Writing { get; set; }
        public bool ThesisDefense { get; set; }
    }
}
