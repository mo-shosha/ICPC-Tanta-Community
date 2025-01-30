using Core.DTO.memberDTO;
using Core.DTO.TeamDTO;
using Core.IServices;
using ICPC_Tanta_Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICPC_Tanta_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemeberServices _memeberServices;
        public MemberController(IMemeberServices memeberServices)
        {
            _memeberServices = memeberServices;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreatMemberDto memberDto)
        {
            await _memeberServices.AddAsync(memberDto);
            return Ok();
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<memberDto>>> GetAll()
        {
            var members= await _memeberServices.GetAllMemberAsync();
            if (members == null)
            {
                return NoContent();
            }
            return Ok(members);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<memberDto>> GetById(int id)
        {
            var members = await _memeberServices.GetMemberByIdAsync(id);
            if (members == null)
            {
                return NoContent();
            }
            return Ok(members);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromForm] memberUpdateDto memberUpdate,int id)
        {

            if (id <= 0 || id != memberUpdate.Id)
            {
                return BadRequest("Invalid or mismatched member ID.");
            }

            var existingMember = await _memeberServices.GetMemberByIdAsync(id);
            if (existingMember == null)
            {
                return NotFound($"Member with ID {id} not found.");
            }

            await _memeberServices.UpdateAsync(memberUpdate);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>Delete(int id)
        {
            if (id <= 0 )
            {
                return BadRequest("Invalid or mismatched member ID.");
            }

            var existingMember = await _memeberServices.GetMemberByIdAsync(id);
            if (existingMember == null)
            {
                return NotFound($"Member with ID {id} not found.");
            }

            await _memeberServices.DeleteAsync(id);
            return NoContent();
        }
    }
}
