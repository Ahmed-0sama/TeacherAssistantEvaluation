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
        public string TaName { get; set; }
        public string TaEmail { get; set; }
        public string PeriodName { get; set; }
        public DateOnly PeriodStartDate { get; set; }
        public DateOnly PeriodEndDate { get; set; }
        public string StatusName { get; set; }
        public int StatusId { get; set; }
        public string? HodStrengths { get; set; }
        public string? HodWeaknesses { get; set; }
        public string? HodReturnComment { get; set; }
        public string? DeanReturnComment { get; set; }
        public string? FinalGrade { get; set; }
        public decimal? StudentSurveyScore { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? DateApproved { get; set; }
        public HodEvaluationDto HodEvaluations { get; set; } = new();
        public decimal EducationalActivityScore { get; set; }        // 50%
        public decimal ScientificActivityScore { get; set; }          // 10%
        public decimal AdministrativeActivityScore { get; set; }      // 10%
        public decimal StudentActivitiesScore { get; set; }           // 10%
        public decimal AcademicAdvisingScore { get; set; }            // 5%
        public decimal PersonalTraitsScore { get; set; }              // 10%
        public decimal TotalScore { get; set; }
        public TASubmissionResponseDto? TaSubmission { get; set; }
    }
}