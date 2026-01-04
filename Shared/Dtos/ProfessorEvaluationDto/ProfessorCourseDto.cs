using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Dtos.ProfessorEvaluationDto
{
    public class ProfessorCourseDto
    {
        [JsonPropertyName("courseCode")]
        public string CourseCode { get; set; }

        [JsonPropertyName("courseName")]
        public string CourseName { get; set; }

        [JsonPropertyName("teachingHours")]
        public int TeachingHours { get; set; }

        [JsonPropertyName("courseProfessorId")]
        public string? CourseProfessorId { get; set; }

        [JsonPropertyName("courseProfessorName")]
        public string? CourseProfessorName { get; set; }

        [JsonPropertyName("gtAs")]
        public List<UserDataDto> GTAs { get; set; } = new List<UserDataDto>();
    }
}
