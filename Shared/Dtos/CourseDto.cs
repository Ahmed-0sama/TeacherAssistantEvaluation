using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class CourseDto
    {
        public string courseCode { get; set; }
        public string courseName { get; set; }
        public int CreditHours { get; set; }
        public int teachingHours { get; set; }
        public int Sections {  get; set; }
        public string courseProfessorId { get; set; }
        public string courseProfessorName { get; set; }

    }
}
