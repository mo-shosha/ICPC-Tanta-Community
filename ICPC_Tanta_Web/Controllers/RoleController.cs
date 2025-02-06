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
                return BadRequest(new { message = "Role name cannot be empty." });

            try
            {
                var result = await _roleService.AddRoleAsync(roleName);
               
                if (result.Succeeded)
                    return Ok(new { message = $"Role '{roleName}' created successfully." });

                return BadRequest(new { message = "Failed to add role.", errors = result.Errors });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }


        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRoleToUser([FromQuery] string userId, [FromQuery] string roleName)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(roleName))
                return BadRequest(new { message = "User ID and Role name cannot be empty." });

            try
            {
                var result = await _roleService.AssignRoleToUserAsync(userId, roleName);
                if (result.Succeeded)
                    return Ok(new { message = $"Role '{roleName}' assigned to user with ID '{userId}' successfully." });

                return BadRequest(new { message = "Failed to assign role.", errors = result.Errors });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }


        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteRole([FromQuery] string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return BadRequest(new { message = "Role name cannot be empty." });

            try
            {
                var result = await _roleService.DeleteRoleAsync(roleName);
                if(result.Succeeded)
                    return Ok(new { message = $"Role '{roleName}' deleted successfully." });
                return BadRequest(new { message = "Failed to remove role.", errors = result.Errors });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }


        [HttpGet("all")]
        public IActionResult GetAllRoles()
        {
            try
            {
                var roles = _roleService.GetAllRoles();
                return Ok(new { roles });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }


        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _roleService.GetAllUser();
                return Ok(new { users });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving users.", error = ex.Message });
            }
        }

    }
}
