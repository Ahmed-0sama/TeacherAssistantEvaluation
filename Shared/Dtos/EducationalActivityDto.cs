using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class EducationalActivityDto
    {
        public List<CourseDto>Courses=new List<CourseDto>();
        public TeachingDataDto TeachingData =new TeachingDataDto();
    }
}
