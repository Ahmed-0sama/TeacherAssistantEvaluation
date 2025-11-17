using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.TASubmissions
{
    public class CreateResearchActivityDto
    {
        public string Title { get; set; }
        public string Journal { get; set; }
        public string Location { get; set; }
        public int PageCount { get; set; }
        public DateOnly ActivityDate { get; set; }
        public int StatusId { get; set; } // Published, Submitted, etc.
        public string Url { get; set; }
    }
}
