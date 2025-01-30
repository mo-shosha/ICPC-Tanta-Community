using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.memberDTO
{
    public class memberUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Role { get; set; }
        public IFormFile? Img { get; set; }
    }
}
