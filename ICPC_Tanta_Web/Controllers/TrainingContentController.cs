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
                return Ok(new { message = "Content created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContentById(int id)
        {
            try
            {
                var content = await _trainingContentServices.GetContentAsyncById(id);
                if (content == null)
                {
                    return NotFound(new { error = "Content not found" });
                }

                return Ok(content);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContent()
        {
            try
            {
                var contents = await _trainingContentServices.GetAllContentAsync();
                return Ok(contents);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContent(int id, [FromBody] ContentUpdateDto contentUpdateDto)
        {
            try
            {
                var existingContent = await _trainingContentServices.GetContentAsyncById(id);
                if (existingContent == null)
                {
                    return NotFound(new { error = "Content not found" });
                }

                await _trainingContentServices.UpdateContentAsync(contentUpdateDto);
                return Ok(new { message = "Content updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContent(int id)
        {
            try
            {
                var existingContent = await _trainingContentServices.GetContentAsyncById(id);
                if (existingContent == null)
                {
                    return NotFound(new { error = "Content not found" });
                }

                await _trainingContentServices.DeleteContentAsync(id);
                return Ok(new { message = "Content deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
