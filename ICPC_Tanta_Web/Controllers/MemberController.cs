using Core.DTO;
using Core.DTO.memberDTO;
using Core.DTO.TeamDTO;
using Core.Entities;
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
            try
            {
                await _memeberServices.AddAsync(memberDto);
                return Ok(ApiResponse<string>.SuccessResponse("Member created successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<memberDto>>> GetAll()
        {
            try
            {
                var members = await _memeberServices.GetAllMemberAsync();
                if (members == null)
                {
                    return NotFound(ApiResponse<string>.ErrorResponse($"No Members found"));
                }
                return Ok(ApiResponse<IEnumerable<memberDto>>.SuccessResponse("Members retrieved successfully.", members));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<memberDto>> GetById(int id)
        {
            try
            {
                var member = await _memeberServices.GetMemberByIdAsync(id);
                if (member == null)
                {
                    return NotFound(ApiResponse<string>.ErrorResponse($"Member with ID {id} not found."));
                }
                return Ok(ApiResponse<memberDto>.SuccessResponse("Member retrieved successfully.", member));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromForm] memberUpdateDto memberUpdate,int id)
        {

            if (id <= 0 || id != memberUpdate.Id)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse("Invalid or mismatched member ID."));
            }

            try
            {
                var existingMember = await _memeberServices.GetMemberByIdAsync(id);
                if (existingMember == null)
                {
                    return NotFound(ApiResponse<string>.ErrorResponse($"Member with ID {id} not found."));
                }

                await _memeberServices.UpdateAsync(memberUpdate);
                return Ok(ApiResponse<string>.SuccessResponse("Member updated successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse("Invalid or mismatched member ID."));
            }

            try
            {
                var existingMember = await _memeberServices.GetMemberByIdAsync(id);
                if (existingMember == null)
                {
                    return NotFound(ApiResponse<string>.ErrorResponse($"Member with ID {id} not found."));
                }

                await _memeberServices.DeleteAsync(id);
                return Ok(ApiResponse<string>.SuccessResponse("Member deleted successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }
    }
}
