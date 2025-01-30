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
            var teams = await _teamService.GetAllAsync();
            return Ok(teams);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDTO>> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }
            var team = await _teamService.GetByIdAsync(id);
            if (team == null)
            {
                return NotFound($"Item with ID {id} was not found.");
            }
            return Ok(team);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateTeamDTO createTeamDTO)
        {
           await _teamService.AddAsync(createTeamDTO);
           return CreatedAtAction(nameof(GetById), new { id = createTeamDTO.TeamName }, createTeamDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateTeamDto updateTeamDTO)
        {
            if (id <= 0 || id != updateTeamDTO.Id)
            {
                return BadRequest("Invalid or mismatched team ID.");  
            }

            var existingTeam = await _teamService.GetByIdAsync(id);
            if (existingTeam == null)
            {
                return NotFound($"Team with ID {id} not found.");  
            }

            await _teamService.UpdateAsync(updateTeamDTO);

            return NoContent();  
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid team ID.");  
            }

            var team = await _teamService.GetByIdAsync(id);
            if (team == null)
            {
                return NotFound($"Team with ID {id} not found.");  
            }

            await _teamService.DeleteAsync(id);
            

            return NoContent();  
        }


        [HttpGet("{id}/members")]
        public async Task<ActionResult<Team>> GetTeamWithMembers(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }

            var teamWithMembers = await _teamService.GetAllByMember(id);
            if (teamWithMembers == null)
            {
                return NotFound($"Team with ID {id} was not found.");
            }

            return Ok(teamWithMembers);
        }


        [HttpGet("{teamId}/members/{year}")]
        public async Task<IActionResult> GetTeamByYear(int teamId, string year)
        {
            var team = await _teamService.GetAllByMemberByYear(teamId, year);

            if (team == null)
                return NotFound(new { Message = "Team not found" });

            return Ok(team);
        }

    }
}
