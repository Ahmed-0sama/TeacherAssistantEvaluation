using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.TASubmissions
{
    public class CreateTASubmissionDto
    {
        public int ActualTeachingLoad { get; set; }
        public int ExpectedTeachingLoad { get; set; }

        public bool HasTechnicalReports { get; set; }
        public bool HasSeminarLectures { get; set; }
        public bool HasAttendingSeminars { get; set; }
        public int? AdvisedStudentCount { get; set; }

        public List<CreateResearchActivityDto> ResearchActivities { get; set; }
    }
}
