using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.ProfessorEvaluationDto
{
    public class ProfessorEvaluationResponseDto
    {
        public int ProfEvalId { get; set; }
        public int EvaluationPeriodId { get; set; }
        public int TaEmployeeId {  get; set; }
        public int ProfessorEmployeeId { get; set; }
        public string ProfessorName { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int OfficeHoursScore { get; set; }
        public int AttendanceScore { get; set; }
        public int PerformanceScore { get; set; }
        public int TotalScore { get; set; }
        public string? Comments { get; set; }
        public bool IsReturned { get; set; }
        public string? HodReturnComment { get; set; }
        public bool IsSubmitted { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public int StatusId { get; set; }
        public string PeriodName { get; set; }
        public string StatusName { get; set; }
    }
}
