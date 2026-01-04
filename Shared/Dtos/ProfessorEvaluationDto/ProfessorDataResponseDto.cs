using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Dtos.ProfessorEvaluationDto
{
    public class ProfessorDataResponseDto
    {
        [JsonPropertyName("professorId")]
        public int ProfessorId { get; set; }

        [JsonPropertyName("professorName")]
        public string? ProfessorName { get; set; }

        [JsonPropertyName("semesterCode")]
        public int SemesterCode { get; set; }

        [JsonPropertyName("semesterYear")]
        public int SemesterYear { get; set; }

        [JsonPropertyName("semesterNumber")]
        public int SemesterNumber { get; set; }

        [JsonPropertyName("courses")]
        public List<ProfessorCourseDto> Courses { get; set; } = new List<ProfessorCourseDto>();
    }

}