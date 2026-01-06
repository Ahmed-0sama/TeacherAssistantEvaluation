using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.ProfessorEvaluationDto
{
    public class CreateProfessorEvaluationDto
    {
        [Required(ErrorMessage = "EvaluationPeriodId is required")]
        public int EvaluationPeriodId { get; set; }
         [Required(ErrorMessage = "TA Employee ID is required")]
        public int TaEmployeeId { get; set; }

        [Required(ErrorMessage = "Professor Employee ID is required")]
        public int ProfessorEmployeeId { get; set; }

        [Required(ErrorMessage = "Course code is required")]
        [StringLength(20, ErrorMessage = "Course code must be less than 20 characters")]
        public string CourseCode { get; set; }

        [Required(ErrorMessage = "Course name is required")]
        [StringLength(200, ErrorMessage = "Course name must be less than 200 characters")]
        public string CourseName { get; set; }
        [Required(ErrorMessage = "Semester name is required")]
        public string SemesterName { get; set; }
        public int SemesterCode { get; set; }
        [Required(ErrorMessage = "Office hours score is required")]
        [Range(0, 10, ErrorMessage = "Office hours score must be between 0 and 10")]
        public int OfficeHoursScore { get; set; }

        [Required(ErrorMessage = "Attendance score is required")]
        [Range(0, 5, ErrorMessage = "Attendance score must be between 0 and 5")]
        public int AttendanceScore { get; set; }

        [Required(ErrorMessage = "Performance score is required")]
        [Range(0, 15, ErrorMessage = "Performance score must be between 0 and 15")]
        public int PerformanceScore { get; set; }

        [StringLength(1000, ErrorMessage = "Comments must be less than 1000 characters")]
        public string? Comments { get; set; }
    }
}
