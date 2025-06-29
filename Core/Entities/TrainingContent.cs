using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class TrainingContent
    {
        public int Id { get; set; }   

        [Required]
        public string Title { get; set; }

        public int WeekNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        // شرح
        public string? ExplanationLink { get; set; }
        public string? ExplanationBy { get; set; }

        // أبسولف
        public string? UpsolveLink { get; set; }
        public string? UpsolveBy { get; set; }

        // الشيت
        public string? SheetLink { get; set; }

        public ICollection<AnotherLink> AnotherLinks { get; set; } = new List<AnotherLink>();


        public int TrainingLevelId { get; set; }

        public TrainingLevel TrainingLevel { get; set; }

        //public int? ContentCategoryId { get; set; }
        //public ContentCategory ContentCategory { get; set; }
    }
}
