using Azure;
using Core.DTO;
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
            try
            {
                if (id <= 0)
                {
                    return BadRequest(ApiResponse<string>.ErrorResponse("Invalid ID provided."));
                }
                var item = await _eventService.GetByIdAsync(id);
                if (item == null)
                {
                    return NotFound(ApiResponse<string>.ErrorResponse($"Event with ID {id} not found."));
                }
                return Ok(ApiResponse<EventWithSchedulesDto>.SuccessResponse("Event retrieved successfully.", item));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _eventService.GetAllAsync();
                return Ok(ApiResponse<IEnumerable<EventWithSchedulesDto>>.SuccessResponse("Events retrieved successfully.", data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] EventCreateDto createEventDto)
        {
            try
            {
                await _eventService.AddAsync(createEventDto);
                return Ok(ApiResponse<EventCreateDto>.SuccessResponse("Event created successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,[FromBody]EventUpdateDto updateEventDto)
        {

            try
            {
                if (id <= 0)
                {
                    return BadRequest(ApiResponse<string>.ErrorResponse("Invalid ID."));
                }
                var item = await _eventService.GetByIdAsync(id);
                if (item == null)
                {
                    return NotFound(ApiResponse<string>.ErrorResponse($"Event with ID {id} not found."));
                }

                updateEventDto.Id = id;
                await _eventService.UpdateAsync(updateEventDto);
                return Ok(ApiResponse<string>.SuccessResponse("Event updated successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(ApiResponse<string>.ErrorResponse("Invalid ID."));
                }
                var item = await _eventService.GetByIdAsync(id);
                if (item == null)
                {
                    return NotFound(ApiResponse<string>.ErrorResponse($"Event with ID {id} not found."));
                }
                await _eventService.DeleteAsync(id);
                return Ok(ApiResponse<string>.SuccessResponse("Event deleted successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }
    }
}
