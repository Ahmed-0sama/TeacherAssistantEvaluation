using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.ProfessorEvaluationDto
{
    public class PendingTAEvaluationDto
    {
        public int EvaluationId { get; set; }
        public int TaEmployeeId { get; set; }
        public string TaEmployeeName { get; set; }
        public string PeriodName { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public DateTime SubmittedDate { get; set; }
        public int DaysPending { get; set; }
        public bool HasExistingEvaluation { get; set; }
        public int? ExistingProfEvalId { get; set; }
    }
}
