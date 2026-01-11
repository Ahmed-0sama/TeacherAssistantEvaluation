using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.ProfessorEvaluationDto
{
    public class ProfessorReminderInfo
    {
        public int ProfessorId { get; set; }
        public string ProfessorName { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int SemesterCode { get; set; }    
        public string SemesterName { get; set; }
    }
}
