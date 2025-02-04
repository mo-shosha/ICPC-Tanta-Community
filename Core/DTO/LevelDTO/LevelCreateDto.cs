using Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.LevelDTO
{
    public class LevelCreateDto
    {
        [Required(ErrorMessage = "Level name is required.")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Level name must be between 3 and 10 characters.")]
        [SafeText]

        public string LevelName { get; set; }
        [Required(ErrorMessage = "Level Description is required.")]
        [SafeText]
        public string Description { get; set; }
    }
}
