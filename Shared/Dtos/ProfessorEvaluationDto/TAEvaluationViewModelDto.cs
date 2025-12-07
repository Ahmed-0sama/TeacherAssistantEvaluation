using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.ProfessorEvaluationDto
{
    public class TAEvaluationViewModelDto
    {
        // From Evaluation API
        public int EvaluationPeriodId { get; set; }
        public int PeriodId { get; set; }
        public string PeriodName { get; set; } = string.Empty;

        // CHANGED: TAEmployeeId is now string (was int)
        public int TAEmployeeId { get; set; }

        // From Professor Evaluation Status
        public int StatusId { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public bool CanEdit { get; set; }

        // From TA Details API (to be fetched)
        public string TAName { get; set; } = string.Empty;

        // REMOVED: WorkId (redundant - use TAEmployeeId directly)

        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string SemesterName { get; set; } = string.Empty;
        public int NumberOfClasses { get; set; }

        // From Professor Evaluation (if exists)
        public int? ProfEvalId { get; set; }
        public int? ProfessorEvaluationTotal { get; set; }

        public bool IsEvaluated => ProfEvalId.HasValue;
    }
}
