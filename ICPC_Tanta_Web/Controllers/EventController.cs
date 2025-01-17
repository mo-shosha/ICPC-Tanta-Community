using Core.DTO.EventDTO;
using Core.IServices;
using ICPC_Tanta_Web.DTO.NewsDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICPC_Tanta_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }
            var item= await _eventService.GetByIdAsync(id);
            if (item == null) 
            {
                return NotFound($"Item with ID {id} was not found.");
            }
            return Ok(item);

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _eventService.GetAllAsync();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] EventCreateDto createEventDto)
        {
            await _eventService.AddAsync(createEventDto);
            return CreatedAtAction(nameof(GetById), new { id = createEventDto.Title }, createEventDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,[FromBody]EventUpdateDto updateEventDto)
        {

            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }
            var item = await _eventService.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound($"Item with ID {id} was not found.");
            }

            updateEventDto.Id = id;
            await _eventService.UpdateAsync(updateEventDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }
            var item = await _eventService.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound($"Item with ID {id} was not found.");
            }
            await _eventService.DeleteAsync(id);
            return Ok();
        }
    }
}
