using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.InfoDTO
{
    public class CreateInfoDto
    {
        [Required]
        public string FacebookUrl { get; set; }
        [Required]
        public string YoutubeUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? InstagramUrl { get; set; }
        [Required]
        public string LinkedInUrl { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
    

}
