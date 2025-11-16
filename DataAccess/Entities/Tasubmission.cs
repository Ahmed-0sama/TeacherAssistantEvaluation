using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Tasubmission
{
    public int SubmissionId { get; set; }

    public int EvaluationId { get; set; }

    public int ActualTeachingLoad { get; set; }

    public int ExpectedTeachingLoad { get; set; }

    public bool HasTechnicalReports { get; set; }

    public bool HasSeminarLectures { get; set; }

    public bool HasAttendingSeminars { get; set; }

    public bool IsInAcademicAdvisingCommittee { get; set; }

    public bool IsInSchedulingCommittee { get; set; }

    public bool IsInQualityAssuranceCommittee { get; set; }

    public bool IsInLabEquipmentCommittee { get; set; }

    public bool IsInExamOrganizationCommittee { get; set; }

    public bool IsInSocialOrSportsCommittee { get; set; }

    public bool ParticipatedInSports { get; set; }

    public bool ParticipatedInSocial { get; set; }

    public bool ParticipatedInCultural { get; set; }

    public int? AdvisedStudentCount { get; set; }

    public virtual Evaluation Evaluation { get; set; } = null!;

    public virtual ICollection<ResearchActivity> ResearchActivities { get; set; } = new List<ResearchActivity>();
}
