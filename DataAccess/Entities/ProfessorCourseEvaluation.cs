using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class ProfessorCourseEvaluation
{
    public int ProfEvalId { get; set; }
    public int EvaluationPeriodId { get; set; }
    public int TaEmployeeId { get; set; }
    public int ProfessorEmployeeId { get; set; }
    public string CourseCode { get; set; } = null!;
    public string CourseName { get; set; } = null!;
    public string SemesterName { get; set; }
    public int OfficeHoursScore { get; set; }
    public int AttendanceScore { get; set; }
    public int PerformanceScore { get; set; }
    public int? TotalScore { get; set; }
    public string? Comments { get; set; }
    public bool IsReturned { get; set; }
    public string? HodReturnComment { get; set; }
    public int StatusId { get; set; }

    public virtual EvaluationPeriod EvaluationPeriod { get; set; } = null!;
    public virtual EvaluationStatus Status { get; set; } = null!;
}
