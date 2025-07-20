using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace Core.Validation
{
    public class IsImageAttribute : ValidationAttribute
    {
        private readonly string[] _validTypes = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file == null)
                return ValidationResult.Success; 

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!_validTypes.Contains(extension))
            {
                return new ValidationResult("Only image files are allowed (jpg, png, gif, bmp).");
            }

            return ValidationResult.Success;
        }
    }
}
