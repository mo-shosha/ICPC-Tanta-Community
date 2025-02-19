using Core.DTO;
using Core.IServices;
using ICPC_Tanta_Web.DTO.NewsDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ICPC_Tanta_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsDto>>> GetAll()
        {
            try
            {
                var data = await _newsService.GetAllAsync();
                if (data == null || !data.Any())
                    return Ok(ApiResponse<IEnumerable<NewsDto>>.SuccessResponse("No news available.", data));

                return Ok(ApiResponse<IEnumerable<NewsDto>>.SuccessResponse("News retrieved successfully.", data));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<NewsDto>> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(ApiResponse<string>.ErrorResponse("Invalid ID provided."));

            try
            {
                var item = await _newsService.GetByIdAsync(id);
                if (item == null)
                    return NotFound(ApiResponse<string>.ErrorResponse($"News with ID {id} not found."));

                return Ok(ApiResponse<NewsDto>.SuccessResponse("News retrieved successfully.", item));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreateNewsDto createNewsDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.ErrorResponse($"Invalid data { ModelState}"));

            try
            {
                var user = User;
                string author = user?.Identity?.IsAuthenticated == true
                    ? user.FindFirst(ClaimTypes.GivenName)?.Value ?? user.Identity.Name
                    : "ICPC Tanta Team";

                string authorId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "System";

                await _newsService.AddAsync(createNewsDto, author, authorId);
                return Ok(ApiResponse<string>.SuccessResponse("News added successfully."));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<string>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse( ex.Message));
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateNewsDto updateNewsDto)
        {
            if (id <= 0)
                return BadRequest(ApiResponse<string>.ErrorResponse("Invalid ID provided."));

            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.ErrorResponse($"Invalid data {ModelState}"));

            try
            {
                await _newsService.UpdateAsync(updateNewsDto);
                return Ok(ApiResponse<string>.SuccessResponse("News updated successfully."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<string>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(ApiResponse<string>.ErrorResponse("Invalid ID provided."));

            try
            {
                await _newsService.DeleteAsync(id);
                return Ok(ApiResponse<string>.SuccessResponse("News deleted successfully."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<string>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest(ApiResponse<string>.ErrorResponse("Search keyword cannot be empty."));

            try
            {
                var results = await _newsService.SearchAsync(keyword);
                if (!results.Any())
                    return NotFound(ApiResponse<IEnumerable<NewsDto>>.SuccessResponse("No matching results found.", results));

                return Ok(ApiResponse<IEnumerable<NewsDto>>.SuccessResponse("Search results retrieved successfully.", results));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }
    }
}
