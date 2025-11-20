using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.TASubmissions
{
    public class UpdateTASubmissionsDto
    {
        public int ActualTeachingLoad { get; set; }
        public int ExpectedTeachingLoad { get; set; }
        public bool HasTechnicalReports { get; set; }
        public bool HasSeminarLectures { get; set; }
        public bool HasAttendingSeminars { get; set; }
        public int? AdvisedStudentCount { get; set; }

        public bool IsInAcademicAdvisingCommittee { get; set; }
        public bool IsInSchedulingCommittee { get; set; }
        public bool IsInQualityAssuranceCommittee { get; set; }
        public bool IsInLabEquipmentCommittee { get; set; }
        public bool IsInExamOrganizationCommittee { get; set; }
        public bool IsInSocialOrSportsCommittee { get; set; }

        public bool ParticipatedInSports { get; set; }
        public bool ParticipatedInSocial { get; set; }
        public bool ParticipatedInCultural { get; set; }

        public List<CreateResearchActivityDto> ResearchActivities { get; set; }
    }

}
