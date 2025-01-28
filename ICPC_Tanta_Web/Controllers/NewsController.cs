using Core.DTO;
using Core.IServices;
using ICPC_Tanta_Web.DTO.NewsDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

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
            var data = await _newsService.GetAllAsync();
            if (data == null || !data.Any())
                return NoContent(); 

            return Ok(data);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<NewsDto>> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid ID provided." });

            var item = await _newsService.GetByIdAsync(id);
            if (item == null)
                return NotFound(new { Message = $"News with ID {id} not found." });

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreateNewsDto createNewsDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _newsService.AddAsync(createNewsDto);
                return CreatedAtAction(nameof(GetById), new { id = createNewsDto.Title }, createNewsDto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
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
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest(new { Message = "Search keyword cannot be empty." });

            var results = await _newsService.SearchAsync(keyword);
            if (!results.Any())
                return NotFound(new { Message = "No matching results found." });

            return Ok(results);
        }
    }
}
