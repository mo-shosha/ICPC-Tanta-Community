using Core.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.LevelDTO
{
    public class LevelUpdateDto
    {
        [Required]
        public int Id {  get; set; }
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Level name must be between 3 and 10 characters.")]
        [SafeText]
        public string? LevelName { get; set; }

        [SafeText]
        public string? Description { get; set; }

        public IFormFile? Image { get; set; }
    }
}
