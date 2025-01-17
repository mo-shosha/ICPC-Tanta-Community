using Core.DTO;
using Core.IServices;
using ICPC_Tanta_Web.DTO.NewsDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<ActionResult<IEnumerable<EventWithSchedulesDto>>> GetAll()
        {
            var data = await _newsService.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventWithSchedulesDto>> GetById(int id)
        {
            if (id <= 0)  
            {
                return BadRequest("Invalid ID.");
            }

            var item = await _newsService.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound($"Item with ID {id} was not found.");
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromForm] CreateNewsDto createNewsDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _newsService.AddAsync(createNewsDto);
            return CreatedAtAction(nameof(GetById), new { id = createNewsDto.Title }, createNewsDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult>Update(int id, [FromForm] UpdateNewsDto updateNewsDto)
        {
            if (id <= 0) return BadRequest("Invalid ID.");

            var item = await _newsService.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound($"Item with ID {id} was not found.");
            }
            updateNewsDto.Id = id;
            await _newsService.UpdateAsync(updateNewsDto);
            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid ID.");
            var item = await _newsService.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound($"Item with ID {id} was not found.");
            }
            await _newsService.DeleteAsync(id);
            return Ok();
        }


        // Search news
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            var results = await _newsService.SearchAsync(keyword);
            return Ok(results);
        }
    }
}
