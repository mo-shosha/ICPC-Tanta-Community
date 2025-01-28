using Microsoft.AspNetCore.Http;

namespace ICPC_Tanta_Web.DTO.NewsDTO
{
    public class CreateNewsDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        //public string Status { get; set; }
        public string Author { get; set; }
        public IFormFile Image { get; set; }

    }
}
