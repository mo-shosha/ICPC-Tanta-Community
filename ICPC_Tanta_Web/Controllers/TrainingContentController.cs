using Core.DTO.ContentDTO;
using Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICPC_Tanta_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingContentController : ControllerBase
    {
        private readonly ITrainingContentServices _trainingContentServices;

        public TrainingContentController(ITrainingContentServices trainingContentServices)
        {
            _trainingContentServices = trainingContentServices;
        }

        [HttpPost]

        public async Task<IActionResult> Create([FromBody] ContentCreateDto contentCreateDto)
        {
            try
            {
                string author = User.Identity?.Name ?? "ICPC Tanta ";
                if (contentCreateDto.Auther == null)
                {
                    contentCreateDto.Auther = author;
                }
                await _trainingContentServices.CreateContentAsync(contentCreateDto);
                return Ok( new { message = "Content created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
