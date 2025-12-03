using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class HodEvaluationTeachingActivityDto
    {
        public int? PreparingPracticalNotes { get; set; }

        public int? PreparingNewTeachingAids { get; set; }

        public int? AssistingInPracticalExperiments { get; set; }

        public int? ParticipatingInOrganizingCourses { get; set; }

        public int? OtherAssignedTeachingActivities { get; set; }


        public decimal TotalDirectTeachingActivities { get; set; }
        public decimal TotalTeachingActivityScore { get; set; }
    }
}
