using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class TeachingDataDto
    {
        public int semestercode { get; set; }
        public int semesterYear { get; set; }
        public int semesterNumber { get; set; }
        public string semestername { get; set; }
        public int ActualTeachingLoad { get; set; }      
        public int ExpectedTeachingLoad { get; set; }    
        public int ClassCount { get; set; }
        public List<CourseDto> courses { get; set; } = new List<CourseDto>();
        public decimal score { get; set; }
    }
}
