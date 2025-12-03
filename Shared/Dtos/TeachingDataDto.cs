using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class TeachingDataDto
    {
        public int ActualTeachingLoad { get; set; }      
        public int ExpectedTeachingLoad { get; set; }    
        public int ClassCount { get; set; }
        public int? gradegiven { get; set; }
    }
}
