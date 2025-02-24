using Core.DTO;
using Core.DTO.AccountDTO;
using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ICPC_Tanta_Web.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddRole([FromBody] string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return BadRequest(ApiResponse<string>.ErrorResponse("Role name cannot be empty."));

            try
            {
                var result = await _roleService.AddRoleAsync(roleName);
                return result.Succeeded
                    ? Ok(ApiResponse<string>.SuccessResponse($"Role '{roleName}' created successfully."))
                    : BadRequest(ApiResponse<string>.ErrorResponse("Failed to add role."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }


        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRoleToUser([FromQuery] string userId, [FromQuery] string roleName)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(roleName))
                return BadRequest(ApiResponse<string>.ErrorResponse("User ID and Role name cannot be empty."));

            try
            {
                var result = await _roleService.AssignRoleToUserAsync(userId, roleName);
                return result.Succeeded
                    ? Ok(ApiResponse<string>.SuccessResponse($"Role '{roleName}' assigned to user '{userId}' successfully."))
                    : BadRequest(ApiResponse<string>.ErrorResponse("Failed to assign role."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }

        }


        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteRole([FromQuery] string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return BadRequest(ApiResponse<string>.ErrorResponse("Role name cannot be empty."));

            try
            {
                var result = await _roleService.DeleteRoleAsync(roleName);
                return result.Succeeded
                    ? Ok(ApiResponse<string>.SuccessResponse($"Role '{roleName}' deleted successfully."))
                    : BadRequest(ApiResponse<string>.ErrorResponse("Failed to remove role."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }

        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = _roleService.GetAllRoles();
                return Ok(ApiResponse<IEnumerable<IdentityRole>>.SuccessResponse("Roles retrieved successfully.", roles));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }



        //[HttpGet("users")]
        //public async Task<IActionResult> GetAllUsers()
        //{
        //    try
        //    {
        //        var users = await _roleService.GetAllUser();
        //        return Ok(ApiResponse<IEnumerable<Userinfo>>.SuccessResponse("Users retrieved successfully.", users));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving users.", error = ex.Message });
        //    }
        //}

    }
}
