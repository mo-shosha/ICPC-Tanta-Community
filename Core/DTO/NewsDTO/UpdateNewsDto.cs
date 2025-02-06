using Core.Validation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ICPC_Tanta_Web.DTO.NewsDTO
{
    public class UpdateNewsDto
    {
        public int Id { get; set; }

        [SafeText]
        public string? Title { get; set; }

        [SafeText]
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
    }
}
