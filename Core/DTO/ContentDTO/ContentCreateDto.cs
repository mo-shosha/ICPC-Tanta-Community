using Core.Entities;
using Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ContentDTO
{
    public class ContentCreateDto
    {
        [Required(ErrorMessage = "Content Title is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Content Title must be between 3 and 50 characters.")]
        
        public string Title { get; set; }

        [Required]
        public int LevelId { get; set; }
        public int WeekNumber { get; set; }

        // Explain
        [Url(ErrorMessage = "Invalid URL format.")]
        public string? ExplanationLink { get; set; }
        public string? ExplanationBy { get; set; }

        // Upsolve
        [Url(ErrorMessage = "Invalid URL format.")]
        public string? UpsolveLink { get; set; }
        public string? UpsolveBy { get; set; }

        // Sheet
        [Url(ErrorMessage = "Invalid URL format.")]
        public string? SheetLink { get; set; }

        public List<AnotherLinkDto> AnotherLinks { get; set; } = new();


    }

    public class AnotherLinkDto
    {
        public string Title { get; set; }
        [Url(ErrorMessage = "Invalid URL format.")]
        public string Url { get; set; }
    }
}
