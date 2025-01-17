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

        public string Description { get; set; }   

        public string ContentUrl { get; set; }   
        public int TrainingLevelId { get; set; }

        public TrainingLevel TrainingLevel { get; set; }
    }
}
