using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.TASubmissions
{
    public class TASubmissionResponseDto
    {
        public int SubmissionId { get; set; }
        public int EvaluationId { get; set; }
        public int ActualTeachingLoad { get; set; }
        public int ExpectedTeachingLoad { get; set; }
        public bool HasTechnicalReports { get; set; }
        public bool HasSeminarLectures { get; set; }
        public bool HasAttendingSeminars { get; set; }

        public bool IsInAcademicAdvisingCommittee { get; set; }
        public bool IsInSchedulingCommittee { get; set; }
        public bool IsInQualityAssuranceCommittee { get; set; }
        public bool IsInLabEquipmentCommittee { get; set; }
        public bool IsInExamOrganizationCommittee { get; set; }
        public bool IsInSocialOrSportsCommittee { get; set; }

        public bool ParticipatedInSports { get; set; }
        public bool ParticipatedInSocial { get; set; }
        public bool ParticipatedInCultural { get; set; }
        public CommiteParticipationDto CommitteeParticipation { get; set; }
        public int? AdvisedStudentCount { get; set; }
        public List<ResearchActivityResponseDto> ResearchActivities { get; set; }
        public DateTime SubmittedDate { get; set; }
    }
}
