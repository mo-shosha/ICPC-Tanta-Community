using Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class News
    {
        public int Id { get; set; }  

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } 
        [Required]
        public string Description { get; set; } 
        public DateTime CreatedDate { get; set; } = DateTime.Now; 
        //[Required]
        //[MaxLength(50)]
        // public string Status { get; set; } // Status of the news (Draft, Published, Archived)

        public string Author { get; set; } // Name of the author or contributor of the news

        public string ImageUrl { get; set; }  
        //public int Likes {  get; set; }

        //public int Dislike {  get; set; }

        public DateTime? PublishedDate { get; set; }

        //public string ApplicationUserId { get; set; }  

        //public ApplicationUser ApplicationUser { get; set; }
    }
}
