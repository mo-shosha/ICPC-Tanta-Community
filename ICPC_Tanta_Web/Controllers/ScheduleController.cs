using Core.DTO;
using Core.DTO.ScheduleDto;
using Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public async Task<IActionResult> GetSchedulesByEventIdAsync(int eventId)
        {
            try
            {
                var schedules = await _scheduleService.GetSchedulesByEventIdAsync(eventId);
                if (schedules == null || !schedules.Any())
                {
                    return NotFound(ApiResponse<string>.ErrorResponse("No schedules found for this event."));
                }
                return Ok(ApiResponse<IEnumerable<ScheduleDtoo>>.SuccessResponse("Schedules retrieved successfully.", schedules));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduleDtoo>>GetByIdAsync(int id)
        {
            try
            {
                var schedule = await _scheduleService.GetByIdAsync(id);
                if (schedule == null)
                {
                    return NotFound(ApiResponse<string>.ErrorResponse("Schedule not found."));
                }
                return Ok(ApiResponse<ScheduleDtoo>.SuccessResponse("Schedule retrieved successfully.", schedule));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        // Create a new schedule
        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] ScheduleCreateDto createScheduleDto)
        {
            try
            {
                await _scheduleService.AddAsync(createScheduleDto);
                return Ok(ApiResponse<string>.SuccessResponse("Schedule created successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }


        // Update an existing schedule
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] ScheduleUpdateDto updateScheduleDto)
        {
            try
            {
                updateScheduleDto.Id = id;
                await _scheduleService.UpdateAsync(updateScheduleDto);
                return Ok(ApiResponse<string>.SuccessResponse("Schedule updated successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }


        // Delete a schedule
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                await _scheduleService.DeleteAsync(id);
                return Ok(ApiResponse<string>.SuccessResponse("Schedule deleted successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }
    }
}
