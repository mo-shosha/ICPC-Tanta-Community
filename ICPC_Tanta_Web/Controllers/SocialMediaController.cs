using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ICPC_Tanta_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialMediaController : ControllerBase
    {
        private readonly ISocialMediaSyncService _socialMediaSyncService;

        public SocialMediaController(ISocialMediaSyncService socialMediaSyncService)
        {
            _socialMediaSyncService = socialMediaSyncService;
        }

        [Authorize(Roles = "Admin,Instructor")]
        [HttpPost("sync-training")]
        public async Task<IActionResult> SyncTrainingVideos()
        {
            await _socialMediaSyncService.SyncYoutubeAsync();
            return Ok(new { message = "Training videos synced successfully." });
        }
    }
}
