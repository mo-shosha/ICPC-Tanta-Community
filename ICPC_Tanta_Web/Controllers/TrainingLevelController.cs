using Core.DTO.LevelDTO;
using Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICPC_Tanta_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingLevelController : ControllerBase
    {
        private readonly ITrainingLevelServices _trainingLevelServices;

        public TrainingLevelController(ITrainingLevelServices trainingLevelServices)
        {
            _trainingLevelServices = trainingLevelServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLevels()
        {
            try
            {
                var levels = await _trainingLevelServices.GetAllLevels();
                return Ok(levels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLevelById(int id)
        {
            try
            {
                var level = await _trainingLevelServices.GetLevelByIdAsync(id);
                return Ok(level);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Level with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateLevel([FromForm] LevelCreateDto levelCreateDto)
        {
            if (levelCreateDto == null)
            {
                return BadRequest("Level data is null.");
            }
            try
            {
                await _trainingLevelServices.CreateLevelAsync(levelCreateDto);
                return Ok(new { message = "Level created successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLevel(int id, [FromForm] LevelUpdateDto levelUpdateDto)
        {
            if (levelUpdateDto == null)
            {
                return BadRequest("Level data is null.");
            }

            if (id != levelUpdateDto.Id)
            {
                return BadRequest("ID mismatch.");
            }

            try
            {
                await _trainingLevelServices.UpdateLevelAsync(levelUpdateDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Level with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLevel(int id)
        {
            try
            {
                await _trainingLevelServices.DeleteLevelAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Level with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("content/{id}")]
        public async Task<IActionResult> GetLevelsWithContent(int id)
        {
            try
            {
                var level = await _trainingLevelServices.GetLevelsWithContentAsync(id);
                return Ok(level);   
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        [HttpGet("content/{id}/{year}")]
        public async Task<IActionResult> GetLevelWithContentByYear(int id,string year)
        {
            try
            {
                var level = await _trainingLevelServices.GetLevelWithContentByYearAsync(id,year);
                return Ok(level);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }
    }
}
