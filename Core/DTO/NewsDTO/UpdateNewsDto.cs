using Microsoft.AspNetCore.Http;

namespace ICPC_Tanta_Web.DTO.NewsDTO
{
    public class UpdateNewsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
    }
}
