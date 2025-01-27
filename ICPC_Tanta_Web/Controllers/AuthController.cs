using Core.DTO.AccountDTO;
using Core.Entities.Identity;
using Core.IServices;
using ICPC_Tanta_Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ICPC_Tanta_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthController(IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }

        // Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var result = await _authService.RegisterAsync(model);
            if (result == null)
                return BadRequest("Registration failed. Please check your details and try again.");

            return Ok(result);
        }

        // Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var result = await _authService.LoginAsync(model);
            if (result == null)
                return Unauthorized("Invalid email or password.");

            return Ok(result);
        }

        // Logout
        //[Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok("Logged out successfully.");
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest("Invalid email confirmation request.");

            var user = await _authService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            var result = await _authService.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                return BadRequest("Email confirmation failed.");

            return Ok("Email confirmed successfully.");
        }
        //[Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userClaims = _httpContextAccessor.HttpContext?.User;
            var userDto = await _authService.GetCurrentUserAsync(userClaims);

            if (userDto == null)
                return NotFound("User not found.");

            return Ok(userDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRoleToUser(AssignRoleDto model)
        {
            if (!User.IsInRole("Admin"))
                return Unauthorized("You do not have permission to assign roles.");

            var result = await _authService.AssignRoleToUserAsync(model.UserId, model.Role);
            if (!result.Succeeded)
                return BadRequest(result.Errors.FirstOrDefault()?.Description);

            return Ok("Role assigned successfully.");
        }
    }
}
