using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class AdminstrativeActivityDto
    {
       public bool IsAcademicGuidanceCommittee { get; set; }
        public bool IsQualityWorksCommittee { get; set; }

        public bool IsExaminationsOrganizingCommittee { get; set; }


        public bool ISchedulingCommittee { get; set; }


        public bool IsLaboratoryEquipmentCommittee { get; set; }


        public bool IsSocialOrSportsActivityCommittees { get; set; }
    }
}
