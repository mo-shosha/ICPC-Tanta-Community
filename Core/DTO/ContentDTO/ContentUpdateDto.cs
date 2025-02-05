using Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ContentDTO
{
    public class ContentUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Content Title must be between 3 and 50 characters.")]
        [SafeText]
        public string Title { get; set; }
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Content Content must be between 3 and 100 characters.")]
        [SafeText]
        public string ContentUrl { get; set; }
    }
}
