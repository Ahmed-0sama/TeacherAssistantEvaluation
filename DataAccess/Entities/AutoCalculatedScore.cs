using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class AutoCalculatedScore
{
    public int ScoreId { get; set; }

    public int EvaluationId { get; set; }

    public int AcademicAdvisingScore { get; set; }

    public int StudentSurveyScore { get; set; }

    public int TechingLoadCompletionScore { get; set; }

    public virtual Evaluation Evaluation { get; set; } = null!;
}
