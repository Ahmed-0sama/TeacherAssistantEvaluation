using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class OtherActivitiesDto
    {
        public bool HasTechnicalReportsOrArticles { get; set; }
        public bool HasGivenLecturesOrSeminars { get; set; }
        public bool HasAttendedScientificSeminars { get; set; }
    }
}
