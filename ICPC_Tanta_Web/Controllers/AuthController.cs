using Core.DTO;
using Core.DTO.AccountDTO;
using Core.Entities.Identity;
using Core.IServices;
using ICPC_Tanta_Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
                return Ok(ApiResponse<UserDto>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }
        

        // Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            try
            {
                var result = await _authService.LoginAsync(model);
                return Ok(ApiResponse<UserDto>.SuccessResponse("Login successful.", result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        // Logout
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authService.LogoutAsync();
                return Ok(ApiResponse<string>.SuccessResponse("Logged out successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(refreshToken))
                    return Unauthorized(ApiResponse<string>.ErrorResponse("Refresh token is missing."));

                var userDto = await _authService.RefreshTokenAsync(refreshToken);
                return Ok(ApiResponse<UserDto>.SuccessResponse("Token refreshed successfully.", userDto));
            }
            catch (Exception ex)
            {
                return Unauthorized(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpPost("revoketoken")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeToken model)
        {
            try
            {
                var token = model.Token ?? Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(token))
                    return BadRequest(ApiResponse<string>.ErrorResponse("Token is required!"));

                var result = await _authService.RevokeTokenAsync(token);
                if (!result)
                    return BadRequest(ApiResponse<string>.ErrorResponse("Invalid token!"));

                return Ok(ApiResponse<string>.SuccessResponse("Token revoked successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }


        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                    return BadRequest(ApiResponse<string>.ErrorResponse("Invalid email confirmation request."));

                var user = await _authService.GetUserByIdAsync(userId);
                if (user == null)
                    return NotFound(ApiResponse<string>.ErrorResponse("User not found."));

                var result = await _authService.ConfirmEmailAsync(user, token);
                if (!result.Succeeded)
                    return BadRequest(ApiResponse<string>.ErrorResponse("Email confirmation failed."));

                return Ok(ApiResponse<string>.SuccessResponse("Email confirmed successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userDto = await _authService.GetCurrentUserAsync(User);
                if (userDto == null)
                    return NotFound(ApiResponse<string>.ErrorResponse("User not found."));

                return Ok(ApiResponse<UserDto>.SuccessResponse("User retrieved successfully.", userDto));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }


        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var result = await _authService.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);

                if (result.Succeeded)
                    return Ok(ApiResponse<string>.SuccessResponse("Password changed successfully. Please log in again."));

                return BadRequest(ApiResponse<string>.ErrorResponse("Password change failed."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }



        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            try
            {
                var result = await _authService.ForgotPasswordAsync(model.Email);
                if (result.Succeeded)
                {
                    return Ok(ApiResponse<UserDto>.SuccessResponse("Password reset link sent to your email."));
                }
                return BadRequest(ApiResponse<string>.ErrorResponse("Error occurred while sending the reset link."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            try
            {
                var result = await _authService.ResetPasswordAsync(model.Email, model.Token, model.NewPassword);
                if (result.Succeeded)
                {
                    return Ok(ApiResponse<UserDto>.SuccessResponse("Password has been reset successfully."));
                }
                return BadRequest(ApiResponse<string>.ErrorResponse("Error occurred while resetting the password."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ChangInfoDto model)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var updatedUser = await _authService.UpdateProfileAsync(userId, model);
                return Ok(ApiResponse<UserDto>.SuccessResponse("Profile updated successfully.", updatedUser));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

    }
}
