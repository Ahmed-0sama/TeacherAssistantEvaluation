using Shared.Dtos.HODEvaluation;
using Shared.Dtos.TASubmissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.DeanDto
{
    public class DeanEvaluationDetailDto
    {
        public int EvaluationId { get; set; }
        public int TaEmployeeId { get; set; }
        public string TaName { get; set; } = string.Empty;
        public string? TaEmail { get; set; }
        public string? PeriodName { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public int StatusId { get; set; }

        // Section Scores - Match backend property names
        public decimal TeachingActivitiesTotal { get; set; }
        public decimal StudentActivitiesTotal { get; set; }
        public decimal PersonalTraitsTotal { get; set; }
        public decimal AdministrativeTotal { get; set; }
        public decimal ScientificActivityScore { get; set; }
        public decimal StudentSurveyScore { get; set; }
        public decimal AcademicAdvisingScore { get; set; }
        public decimal ProfessorAverageCourseScore { get; set; }
        
        // Teaching load completion (0 or 10 points)
        public decimal TeachingLoadCompletionScore { get; set; }

        // Aliases for frontend compatibility
        public int? EducationalActivityScore => (int)Math.Round(TeachingActivitiesTotal);
        public int? ScientificActivityScore2 => (int)Math.Round(ScientificActivityScore);
        public int? AdministrativeActivityScore => (int)Math.Round(AdministrativeTotal);
        public int? StudentActivitiesScore => (int)Math.Round(StudentActivitiesTotal);
        public int? PersonalTraitsScore => (int)Math.Round(PersonalTraitsTotal);

        // Total and Grade
        public decimal TotalScore { get; set; }
        public string FinalGrade { get; set; } = string.Empty;

        // HOD Comments
        public string? HodStrengths { get; set; }
        public string? HodWeaknesses { get; set; }
        public string? HodReturnComment { get; set; }
        public string? DeanReturnComment { get; set; }
    }
}