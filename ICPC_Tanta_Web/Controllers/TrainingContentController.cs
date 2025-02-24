using Core.DTO;
using Core.DTO.ContentDTO;
using Core.Entities;
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
                string author = User.Identity?.Name ?? "ICPC Tanta";
                contentCreateDto.Auther ??= author;

                await _trainingContentServices.CreateContentAsync(contentCreateDto);
                return Ok(ApiResponse<string>.SuccessResponse("Content created successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContentById(int id)
        {
            try
            {
                var content = await _trainingContentServices.GetContentAsyncById(id);
                return content != null
                    ? Ok(ApiResponse<TrainingContent>.SuccessResponse("Content retrieve successfully", content))
                    : NotFound(ApiResponse<string>.ErrorResponse("Content not found."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContent()
        {
            try
            {
                var contents = await _trainingContentServices.GetAllContentAsync();
                return Ok(ApiResponse<IEnumerable<TrainingContent>>.SuccessResponse("Content retrieve successfully", contents));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContent(int id, [FromBody] ContentUpdateDto contentUpdateDto)
        {
            try
            {
                var existingContent = await _trainingContentServices.GetContentAsyncById(id);
                if (existingContent == null)
                    return NotFound(ApiResponse<string>.ErrorResponse("Content not found."));

                await _trainingContentServices.UpdateContentAsync(contentUpdateDto);
                return Ok(ApiResponse<string>.SuccessResponse("Content updated successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContent(int id)
        {
            try
            {
                var existingContent = await _trainingContentServices.GetContentAsyncById(id);
                if (existingContent == null)
                    return NotFound(ApiResponse<string>.ErrorResponse("Content not found."));

                await _trainingContentServices.DeleteContentAsync(id);
                return Ok(ApiResponse<string>.SuccessResponse("Content deleted successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }
    }
}
