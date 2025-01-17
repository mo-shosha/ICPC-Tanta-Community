using Core.DTO.ScheduleDto;
using Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICPC_Tanta_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        // Get all schedules for an event
        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<IEnumerable<ScheduleDtoo>>> GetSchedulesByEventIdAsync(int eventId)
        {
            var schedules = await _scheduleService.GetSchedulesByEventIdAsync(eventId);
            if (schedules == null)
            {
                return NotFound("No schedules found for this event.");
            }
            return Ok(schedules);
        }

        // Create a new schedule
        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] ScheduleCreateDto createScheduleDto)
        {
            await _scheduleService.AddAsync(createScheduleDto);
            return CreatedAtAction(nameof(GetSchedulesByEventIdAsync), new { eventId = createScheduleDto.EventId }, createScheduleDto);
        }

        // Update an existing schedule
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] ScheduleUpdateDto updateScheduleDto)
        {
            updateScheduleDto.Id = id;
            await _scheduleService.UpdateAsync(updateScheduleDto);
            return NoContent();
        }

        // Delete a schedule
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _scheduleService.DeleteAsync(id);
            return NoContent();
        }
    }
}
