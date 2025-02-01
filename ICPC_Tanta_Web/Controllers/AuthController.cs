using Core.DTO.AccountDTO;
using Core.Entities.Identity;
using Core.IServices;
using ICPC_Tanta_Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            try
            {
                var result = await _authService.RegisterAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            try
            {
                var result = await _authService.LoginAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized($"Error: {ex.Message}");
            }
        }

        // Logout
        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authService.LogoutAsync();
                return Ok("Logged out successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
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

        
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;  
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated.");

            var result = await _authService.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
                return Ok("Password changed successfully.");

            return BadRequest("Password change failed.");
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            var result = await _authService.ForgotPasswordAsync(model.Email);
            if (result.Succeeded)
            {
                return Ok("Password reset link sent to your email.");
            }
            return BadRequest("Error occurred while sending the reset link.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var result = await _authService.ResetPasswordAsync(model.Email, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok("Password has been reset successfully.");
            }
            return BadRequest("Error occurred while resetting the password.");
        }
    }
}
