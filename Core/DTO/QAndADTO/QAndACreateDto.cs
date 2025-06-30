using System.ComponentModel.DataAnnotations;

namespace Core.DTO.QAndADTO
{
    public class QAndACreateDto
    {
        [Required(ErrorMessage = "Question is required.")]
        [MinLength(3, ErrorMessage = "Question must be at least 3 characters long.")]
        [RegularExpression(@"^(?!.*(?:http|https|www\.)).*$", ErrorMessage = "Question must not contain links.")]
        public string Question { get; set; } = string.Empty;
    }
}
