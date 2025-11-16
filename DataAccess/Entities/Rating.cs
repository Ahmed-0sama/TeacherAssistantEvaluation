using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Rating
{
    public int RatingId { get; set; }

    public string RatingName { get; set; } = null!;

    public int ScoreValue { get; set; }

    public virtual ICollection<Hodevaluation> Hodevaluations { get; set; } = new List<Hodevaluation>();
}
