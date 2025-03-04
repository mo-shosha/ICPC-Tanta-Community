using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class info
    {
        [Key]
        public int Id {  get; set; }
        public string FacebookUrl { get; set; }
        public string YoutubeUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string LinkedInUrl { get; set; }

         public string BackGroundUrl { get; set; }
         
    }
}
