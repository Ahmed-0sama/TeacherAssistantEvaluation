using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.ProfessorEvaluationDto
{
    public class UpdateProfessorEvaluationDto
    {

           [Required(ErrorMessage = "Course code is required")]
            [StringLength(20)]
            public string CourseCode { get; set; }

            [Required(ErrorMessage = "Course name is required")]
            [StringLength(200)]
            public string CourseName { get; set; }

            public string SemesterName { get; set; }
            [Required]
            [Range(0, 10)]
            public int OfficeHoursScore { get; set; }

            [Required]
            [Range(0, 5)]
            public int AttendanceScore { get; set; }

            [Required]
            [Range(0, 15)]
            public int PerformanceScore { get; set; }

            [StringLength(1000)]
            public string? Comments { get; set; }
        
    }
}
