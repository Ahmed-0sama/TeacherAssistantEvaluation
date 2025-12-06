using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{      
    public partial class EvaluationGradeRange
    {
       
            public int RangeId { get; set; }

            public int MaxGrade { get; set; }

            public int MinGrade { get; set; }

            public string? Description { get; set; }

            public DateOnly ValidFromDate { get; set; }

            public DateOnly ValidToDate { get; set; }
        }
    }

