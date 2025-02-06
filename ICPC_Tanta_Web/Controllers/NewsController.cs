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
                    return NoContent();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An error occurred while fetching news.", Details = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<NewsDto>> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid ID provided." });

            try
            {
                var item = await _newsService.GetByIdAsync(id);
                if (item == null)
                    return NotFound(new { Message = $"News with ID {id} not found." });

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An error occurred while retrieving the news item.", Details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreateNewsDto createNewsDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = User;
                string author = user?.Identity?.IsAuthenticated == true
                    ? user.FindFirst(ClaimTypes.GivenName)?.Value ?? user.Identity.Name
                    : "ICPC Tanta Team";

                string authorId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "System";

                await _newsService.AddAsync(createNewsDto, author, authorId);
                return CreatedAtAction(nameof(GetById), new { id = createNewsDto.Title }, new { Message = "News added successfully." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An error occurred while adding news.", Details = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateNewsDto updateNewsDto)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid ID provided." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _newsService.UpdateAsync(updateNewsDto);
                return Ok(new { Message = "News updated successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An error occurred while updating the news item.", Details = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid ID provided." });

            try
            {
                await _newsService.DeleteAsync(id);
                return Ok(new { Message = "News deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An error occurred while deleting the news item.", Details = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest(new { Message = "Search keyword cannot be empty." });

            try
            {
                var results = await _newsService.SearchAsync(keyword);
                if (!results.Any())
                    return NotFound(new { Message = "No matching results found." });

                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An error occurred while searching for news.", Details = ex.Message });
            }
        }
    }
}
