using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class MastersDto
    {
        public string Major { get; set; }
        public int HoursGained { get; set; }
        public double Grade { get; set; }
        public DateOnly EstimatedGraduationDate { get; set; }
        public bool ResearchPointsCompleted { get; set; }
        public bool SearchInResources { get; set; }
        public bool SearchPlanning { get; set; }
        public bool CollectDataAndAnalyze { get; set; }
        public bool ResearchReportWriting { get; set; }
        public bool ScientificResearchReport { get; set; }
        public double GradeGivenbySupervisor { get; set; }
        public string Status { get; set; }
    }
}
