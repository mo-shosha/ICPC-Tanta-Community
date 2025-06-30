using System.ComponentModel.DataAnnotations;

namespace Core.DTO.QAndADTO
{
    public class QAndACreateAnsDto
    {
        [Required(ErrorMessage = "Answer is required.")]
        [MinLength(3, ErrorMessage = "Answer must be at least 3 characters long.")]
        [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Answer must not be empty or whitespace.")]
        public string Answer { get; set; } = string.Empty;
    }
}
