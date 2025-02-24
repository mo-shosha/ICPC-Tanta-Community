using Core.DTO;
using Core.DTO.TeamDTO;
using Core.Entities;
using Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICPC_Tanta_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDTO>>> GetAll()
        {
            try
            {
                var teams = await _teamService.GetAllAsync();
                return Ok(ApiResponse<IEnumerable<TeamDTO>>.SuccessResponse("Teams retrieved successfully.", teams));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(ApiResponse<string>.ErrorResponse("Invalid team ID."));

            try
            {
                var team = await _teamService.GetByIdAsync(id);
                if (team == null)
                    return NotFound(ApiResponse<string>.ErrorResponse($"Team with ID {id} was not found."));

                return Ok(ApiResponse<TeamDTO>.SuccessResponse("Team retrieved successfully.", team));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateTeamDTO createTeamDTO)
        {
            try
            {
                await _teamService.AddAsync(createTeamDTO);
                return Ok(ApiResponse<string>.SuccessResponse("Team created successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateTeamDto updateTeamDTO)
        {
            if (id <= 0 || id != updateTeamDTO.Id)
                return BadRequest(ApiResponse<string>.ErrorResponse("Invalid or mismatched team ID."));

            try
            {
                var existingTeam = await _teamService.GetByIdAsync(id);
                if (existingTeam == null)
                    return NotFound(ApiResponse<string>.ErrorResponse($"Team with ID {id} not found."));

                await _teamService.UpdateAsync(updateTeamDTO);
                return Ok(ApiResponse<string>.SuccessResponse("Team updated successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(ApiResponse<string>.ErrorResponse("Invalid team ID."));

            try
            {
                var team = await _teamService.GetByIdAsync(id);
                if (team == null)
                    return NotFound(ApiResponse<string>.ErrorResponse($"Team with ID {id} not found."));

                await _teamService.DeleteAsync(id);
                return Ok(ApiResponse<string>.SuccessResponse("Team deleted successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }


        [HttpGet("{id}/members")]
        public async Task<ActionResult<Team>> GetTeamWithMembers(int id)
        {
            if (id <= 0)
                return BadRequest(ApiResponse<string>.ErrorResponse("Invalid team ID."));

            try
            {
                var teamWithMembers = await _teamService.GetAllByMember(id);
                if (teamWithMembers == null)
                    return NotFound(ApiResponse<string>.ErrorResponse($"Team with ID {id} was not found."));

                return Ok(ApiResponse<Team>.SuccessResponse("Team with members retrieved successfully.", teamWithMembers));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }


        [HttpGet("{teamId}/members/{year}")]
        public async Task<IActionResult> GetTeamByYear(int teamId, string year)
        {
            if (teamId <= 0 || string.IsNullOrWhiteSpace(year))
                return BadRequest(ApiResponse<string>.ErrorResponse("Invalid team ID or year."));

            try
            {
                var team = await _teamService.GetAllByMemberByYear(teamId, year);
                if (team == null)
                    return NotFound(ApiResponse<string>.ErrorResponse("Team not found."));

                return Ok(ApiResponse<Team>.SuccessResponse("Team for the specified year retrieved successfully.", team));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

    }
}
