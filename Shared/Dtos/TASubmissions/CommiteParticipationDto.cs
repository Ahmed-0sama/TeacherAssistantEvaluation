using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.TASubmissions
{
    public class CommiteParticipationDto
    {
            public bool IsInAcademicAdvisingCommittee { get; set; }
            public bool IsInSchedulingCommittee { get; set; }
            public bool IsInQualityAssuranceCommittee { get; set; }
            public bool IsInLabEquipmentCommittee { get; set; }
            public bool IsInExamOrganizationCommittee { get; set; }
            public bool IsInSocialOrSportsCommittee { get; set; }
            public bool ParticipatedInSports { get; set; }
            public bool ParticipatedInSocial { get; set; }
            public bool ParticipatedInCultural { get; set; }
        
    }
}
