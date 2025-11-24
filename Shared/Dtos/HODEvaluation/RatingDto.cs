using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.HODEvaluation
{
    public class RatingDto
    {
        public int RatingId { get; set; }
        public string RatingName { get; set; } = null!;
        public int ScoreValue { get; set; }
    }
}
