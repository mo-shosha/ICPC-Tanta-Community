using Azure.Core;
using Core.DTO;
using Core.DTO.AccountDTO;
using Core.Entities;
using Core.Entities.Identity;
using Core.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ICPC_Tanta_Web.Services
{
    public class AuthService: IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenServices _tokenServices;
        private readonly ICodeforcesService _codeforcesService;
        private readonly IEmailService _emailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenServices tokenServices,
            ICodeforcesService codeforcesService,
            IEmailService emailService,
            RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenServices = tokenServices;
            _codeforcesService = codeforcesService;
            _emailService = emailService;
            _roleManager = roleManager;
            _httpContextAccessor=httpContextAccessor;
        }

        public async Task<string> RegisterAsync(RegisterDto model)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                    throw new Exception("An account with this email already exists.");

                var codeforcesUserInfo = await _codeforcesService.GetUserInfoAsync(model.CodeForcesHandel);
                if (codeforcesUserInfo == null)
                    throw new Exception("Invalid Codeforces handle.");

                var newUser = new ApplicationUser
                {
                    FullName = model.DisplayName,
                    Email = model.Email,
                    UserName = model.Email.Split('@')[0],
                    CodeForcesHandel = model.CodeForcesHandel,
                    PhoneNumber = model.PhoneNumber,
                };

               
                var result = await _userManager.CreateAsync(newUser, model.Password);

                if (!result.Succeeded)
                    throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));

                await _userManager.AddToRoleAsync(newUser, "User");

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                var confirmationLink = $"https://icpc-tanta.runasp.net/api/Auth/ConfirmEmail?userId={newUser.Id}&token={Uri.EscapeDataString(token)}";

                _ = Task.Run(async () =>
                {
                    await _emailService.SendEmailAsync(
                        newUser.Email,
                        "Confirm Your Email",
                        $@"
                        <div style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: auto; border: 1px solid #ddd; border-radius: 8px; padding: 20px;'>
                            <h2 style='color: #007bff;'>Confirm Your Email Address</h2>
                            <p>Hello {newUser.FullName},</p>
                            <p>Thank you for registering! Please confirm your email address to activate your account.</p>
                            <p style='text-align: center;'>
                                <a href='{confirmationLink}' style='display: inline-block; padding: 10px 20px; background-color: #007bff; color: #fff; text-decoration: none; border-radius: 5px; font-weight: bold;'>
                                    Confirm Email
                                </a>
                            </p>
                            <p>If you did not create an account, please ignore this email.</p>
                            <p>Thank you,<br/>The Team</p>
                            <hr style='margin-top: 20px; border: none; border-top: 1px solid #ddd;'/>
                            <p style='font-size: 12px; color: #888;'>This email was sent to {newUser.Email}. If you have any questions, contact us at support@example.com.</p>
                        </div>"
                    );
                });

                return "User registered successfully. Please confirm your email to activate your account and try login agin.";
            
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in RegisterAsync: {ex.Message}");
            }
        }

        public async Task<UserDto> LoginAsync(LoginDto model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                    throw new UnauthorizedAccessException("Invalid credentials. Please check your email and password.");

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (!result.Succeeded)
                    throw new UnauthorizedAccessException("Invalid credentials. Please check your email and password.");

                var accessToken = await _tokenServices.CreateTokenAsync(user, _userManager);
                var newRefreshToken = _tokenServices.GenerateRefreshToken();

                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                if (activeRefreshToken == null)
                {
                    user.RefreshTokens.Add(newRefreshToken);
                    activeRefreshToken = newRefreshToken;
                }

                await _userManager.UpdateAsync(user);

                SetRefreshTokenInCookie(activeRefreshToken.Token, activeRefreshToken.ExpiresOn);

                var codeforcesUserInfo = await _codeforcesService.GetUserInfoAsync(user.CodeForcesHandel);

                var userRoles = await _userManager.GetRolesAsync(user);

                return new UserDto
                {
                    Id = user.Id,
                    DisplayName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber ?? "Unknown",
                    Handle = user.CodeForcesHandel ?? "Unknown",
                    Rating = codeforcesUserInfo?.Rating ?? 0,
                    Rank = codeforcesUserInfo?.Rank ?? "Unknown",
                    TitlePhoto = codeforcesUserInfo?.TitlePhoto ?? "default-avatar.png",
                    Roles = userRoles.ToList(),
                    Token = accessToken,
                    RefreshToken = activeRefreshToken.Token,
                    RefreshTokenExpiration = activeRefreshToken.ExpiresOn
                };
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException($"Login failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred in LoginAsync: {ex.Message}");
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                    throw new InvalidOperationException("No active HTTP context found.");

                httpContext.Response.Cookies.Delete("refreshToken");
                await _signInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in LogoutAsync: {ex.Message}");
            }
        }


        public async Task<UserDto> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken));
                if (user == null)
                    throw new UnauthorizedAccessException("Invalid refresh token.");

                var storedToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken);
                if (storedToken == null || !storedToken.IsActive)
                    throw new UnauthorizedAccessException("Refresh token is expired or revoked.");

                // إزالة التوكن القديم 
                user.RefreshTokens.Remove(storedToken);

                // إنشاء التوكنات الجديدة
                var newAccessToken = await _tokenServices.CreateTokenAsync(user, _userManager);
                var newRefreshToken = _tokenServices.GenerateRefreshToken();

                user.RefreshTokens.Add(newRefreshToken);
                await _userManager.UpdateAsync(user);

                SetRefreshTokenInCookie(newRefreshToken.Token, newRefreshToken.ExpiresOn);

                // استرجاع بيانات Codeforces
                var codeforcesUserInfo = await _codeforcesService.GetUserInfoAsync(user.CodeForcesHandel);

                // استرجاع الأدوار الخاصة بالمستخدم
                var existingRoles = await _userManager.GetRolesAsync(user);

                return new UserDto
                {
                    Id = user.Id,
                    DisplayName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Handle = user.CodeForcesHandel ?? "Unknown",
                    Rating = codeforcesUserInfo?.Rating ?? 0,
                    Rank = codeforcesUserInfo?.Rank ?? "Unknown",
                    TitlePhoto = codeforcesUserInfo?.TitlePhoto ?? "default-avatar.png",
                    Roles = existingRoles.ToList(),
                    Token = newAccessToken,
                    RefreshToken = newRefreshToken.Token,
                    RefreshTokenExpiration = newRefreshToken.ExpiresOn
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in RefreshTokenAsync: {ex.Message}");
            }
        }
        public async Task<bool> RevokeTokenAsync(string token)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
                if (user == null)
                    return false;

                var refreshToken = user.RefreshTokens.FirstOrDefault(t => t.Token == token);
                if (refreshToken == null || !refreshToken.IsActive)
                    return false;

                user.RefreshTokens.Remove(refreshToken);
                await _userManager.UpdateAsync(user);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in RevokeTokenAsync: {ex.Message}");
            }
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving user: {ex.Message}");
            }
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
        
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token)
        {
            try
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (!result.Succeeded)
                {
                    throw new Exception("Email confirmation failed");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error confirming email: {ex.Message}");
            }
        }

        public async Task<UserDto> GetCurrentUserAsync(ClaimsPrincipal userClaims)
        {
            var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return null;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            var codeforcesUserInfo = await _codeforcesService.GetUserInfoAsync(user.CodeForcesHandel);

            return new UserDto
            {
                DisplayName = user.FullName,
                Email = user.Email,
                Token = await _tokenServices.CreateTokenAsync(user, _userManager),
                Rating = codeforcesUserInfo?.Rating ?? 0,
                Rank = codeforcesUserInfo?.Rank ?? "Unknown",
                TitlePhoto = codeforcesUserInfo?.TitlePhoto ?? "default-avatar.png",
                Handle = codeforcesUserInfo.Handle
            };

        }

        public async Task<string> GetCurrentUserName(ClaimsPrincipal userClaims)
        {
            var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return null;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            return user.FullName;
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return IdentityResult.Failed(new IdentityError { Description = "User not found" });

                var passwordCheck = await _userManager.CheckPasswordAsync(user, newPassword);
                if (passwordCheck)
                    return IdentityResult.Failed(new IdentityError { Description = "New password cannot be the same as the old password" });

                var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
                if (!result.Succeeded)
                    return result;

                user.RefreshTokens.Clear();
                await _userManager.UpdateAsync(user);

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Error: {ex.Message}" });
            }
        }

        public async Task<IdentityResult> ForgotPasswordAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetLink = $"https://icpc-tanta.runasp.net/api/Auth/ResetPassword?userId={user.Id}&token={Uri.EscapeDataString(token)}";

                await _emailService.SendEmailAsync(
                    user.Email,
                    "Reset Your Password",
                    $@"
                    <div style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: auto; border: 1px solid #ddd; border-radius: 8px; padding: 20px;'>
                        <h2 style='color: #007bff;'>Reset Your Password</h2>
                        <p>Hello {user.FullName},</p>
                        <p>We received a request to reset your password. Please click the link below to reset your password:</p>
                        <p style='text-align: center;'>
                            <a href='{resetLink}' style='display: inline-block; padding: 10px 20px; background-color: #007bff; color: #fff; text-decoration: none; border-radius: 5px; font-weight: bold;'>
                                Reset Password
                            </a>
                        </p>
                        <p>If you did not request a password reset, please ignore this email.</p>
                        <p>Thank you,<br/>The Team</p>
                    </div>"
                );

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in ForgotPasswordAsync: {ex.Message}");
            }
        }

        public async Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return IdentityResult.Failed(new IdentityError { Description = "User not found." });
                }

                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                if (!result.Succeeded)
                {
                    throw new Exception("Password reset failed.");
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in ResetPasswordAsync: {ex.Message}");
            }
        }

        public async Task<UserDto> UpdateProfileAsync(string userId, ChangInfoDto model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new Exception("User not found.");

                bool isUpdated = false;

                if (!string.IsNullOrEmpty(model.FullName) && model.FullName != user.FullName)
                {
                    user.FullName = model.FullName;
                    isUpdated = true;
                }

                if (!string.IsNullOrEmpty(model.CodeForcesHandle) && model.CodeForcesHandle != user.CodeForcesHandel)
                {
                    var codeforcesUserInfo = await _codeforcesService.GetUserInfoAsync(model.CodeForcesHandle);
                    if (codeforcesUserInfo == null)
                        throw new Exception("Invalid Codeforces handle.");

                    user.CodeForcesHandel = model.CodeForcesHandle;
                    isUpdated = true;
                }

                if (!string.IsNullOrEmpty(model.PhoneNumber) && model.PhoneNumber != user.PhoneNumber)
                {
                    user.PhoneNumber = model.PhoneNumber;
                    isUpdated = true;
                }

                if (!isUpdated)
                    throw new Exception("No changes detected.");

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    throw new Exception("Failed to update user profile.");

                var existingRoles = await _userManager.GetRolesAsync(user);

                var codeforcesUserInfoUpdated = await _codeforcesService.GetUserInfoAsync(user.CodeForcesHandel);

                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(rt => rt.IsActive);

                if (activeRefreshToken != null)
                {
                    SetRefreshTokenInCookie(activeRefreshToken.Token, activeRefreshToken.ExpiresOn);
                }

                return new UserDto
                {
                    Id = user.Id,
                    DisplayName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Handle = user.CodeForcesHandel ?? "Unknown",
                    Rating = codeforcesUserInfoUpdated?.Rating ?? 0,
                    Rank = codeforcesUserInfoUpdated?.Rank ?? "Unknown",
                    TitlePhoto = codeforcesUserInfoUpdated?.TitlePhoto ?? "default-avatar.png",
                    Roles = existingRoles.ToList(),
                    Token = await _tokenServices.CreateTokenAsync(user, _userManager),
                    RefreshToken = activeRefreshToken?.Token,
                    RefreshTokenExpiration = activeRefreshToken?.ExpiresOn ?? DateTime.MinValue
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in UpdateProfileAsync: {ex.Message}");
            }
        }



        private void SetRefreshTokenInCookie(string token, DateTime expires)
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", token, options);
        }

        
    }
}

