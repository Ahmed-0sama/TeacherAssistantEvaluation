using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.DeanDto
{
    public class DeanActionResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int EvaluationId { get; set; }
        public string NewStatus { get; set; }
    }
}
