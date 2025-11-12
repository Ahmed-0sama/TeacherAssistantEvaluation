using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class TAEvaluationDto
    {
            public int Id { get; set; }               
            public string TAName { get; set; } = "";
            public string WorkId { get; set; } = "";
            public string SubjectName { get; set; } = "";
            public int NumberOfClasses { get; set; }  
            public int grade1 { get; set; }
            public int grade2 { get; set; }
            public int grade3 { get; set; }

            public int? Total { get; set; }

            public string Status
            {
            get; set;        
            }
     }
}
