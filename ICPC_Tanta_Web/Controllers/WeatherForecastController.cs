using ICPC_Tanta_Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace ICPC_Tanta_Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly YoutubeService _youtubeService;

        public WeatherForecastController(YoutubeService youtubeService)
        {
            _youtubeService = youtubeService;
        }

        [HttpGet("videos")]
        public async Task<IActionResult> GetVideos()
        {
            var videos = await _youtubeService.GetLatestVideosAsync();
            return Ok(videos); 
        }
    }
}
