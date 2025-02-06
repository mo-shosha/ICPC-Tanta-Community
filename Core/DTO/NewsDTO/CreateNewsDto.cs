using Core.Validation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ICPC_Tanta_Web.DTO.NewsDTO
{
    public class CreateNewsDto
    {
        [SafeText]
        [Required]
        public string Title { get; set; }

        [SafeText]
        [Required]
        public string Description { get; set; }

        [Required]
        public IFormFile Image { get; set; }

    }
}
