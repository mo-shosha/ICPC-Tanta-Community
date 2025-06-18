using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ContentCategory
    {
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; }

        public ICollection<TrainingContent> TrainingContents { get; set; }
    }
}
