using Core.DTO;
using Core.DTO.AccountDTO;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto model);
        Task<UserDto> LoginAsync(LoginDto model);
        Task LogoutAsync();
        Task<UserDto> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeTokenAsync(string token);
        Task<UserDto> UpdateProfileAsync(string userId, ChangInfoDto model);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token);
        Task<UserDto> GetCurrentUserAsync(ClaimsPrincipal userClaims);
        Task<string>GetCurrentUserName(ClaimsPrincipal userClaims);
        Task<IdentityResult> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
        Task<IdentityResult> ForgotPasswordAsync(string email);
        Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword);
    }
}
