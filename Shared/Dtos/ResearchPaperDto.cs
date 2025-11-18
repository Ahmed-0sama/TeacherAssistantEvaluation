using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class ResearchPaperDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ConferenceOrJournal { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int PageCount { get; set; }
        public DateOnly Date { get; set; }
        public int Status { get; set; }
        public string Url { get; set; } = string.Empty;

    }
}
