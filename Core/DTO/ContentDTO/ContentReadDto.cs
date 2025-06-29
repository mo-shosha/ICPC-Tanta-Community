using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ContentDTO
{
    public class ContentReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int WeekNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ExplanationLink { get; set; }
        public string? ExplanationBy { get; set; }

        public string? UpsolveLink { get; set; }
        public string? UpsolveBy { get; set; }

        public string? SheetLink { get; set; }

        public List<AnotherLinkDto> AnotherLinks { get; set; }
    }

    

}
