using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class StickyNoteDto
    {
        public string Content { get; set; }
        public string AuthorName { get; set; }
    }

    public class StickyNoteCreateDto
    {
        [Required(ErrorMessage = "Content is required.")]
        [MinLength(3, ErrorMessage = "Content must be at least 3 characters long.")]
        [RegularExpression(@"^(?!.*(?:http|https|www\.)).*$", ErrorMessage = "Content must not contain links.")]
        public string Content { get; set; }
    }
}
