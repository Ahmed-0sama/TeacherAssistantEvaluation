using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class StudentActivityDto
    {
        public bool IsCompletedStudentActivity { get; set; }
        public bool IsCompletedSocialActivity { get; set; }
        public bool IsCompletedCulturalActivity { get; set; }
    }
}
