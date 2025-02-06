using Core.DTO.AccountDTO;
using Core.Entities.Identity;
using Core.IServices;
using Microsoft.AspNetCore.Identity;
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
        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenServices tokenServices,
            ICodeforcesService codeforcesService,
            IEmailService emailService,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenServices = tokenServices;
            _codeforcesService = codeforcesService;
            _emailService = emailService;
            _roleManager = roleManager;
        }

        public async Task<UserDto> RegisterAsync(RegisterDto model)
        {
            try
            {
                var newUser = new ApplicationUser
                {
                    FullName = model.DisplayName,
                    Email = model.Email,
                    UserName = model.Email.Split('@')[0],
                    CodeForcesHandel = model.CodeForcesHandel,
                    PhoneNumber = model.PhoneNumber,
                };

                var codeforcesUserInfo = await _codeforcesService.GetUserInfoAsync(model.CodeForcesHandel);
                if (codeforcesUserInfo == null)
                    throw new Exception("Invalid Codeforces handle.");

                var result = await _userManager.CreateAsync(newUser, model.Password);

                if (!result.Succeeded)
                    throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));

                await _userManager.AddToRoleAsync(newUser, "User");

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                var confirmationLink = $"https://icpc-tanta.runasp.net/api/Auth/ConfirmEmail?userId={newUser.Id}&token={Uri.EscapeDataString(token)}";

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

                return new UserDto
                {
                    DisplayName = newUser.FullName,
                    Email = newUser.Email,
                    Token = await _tokenServices.CreateTokenAsync(newUser, _userManager),
                    Rating = codeforcesUserInfo?.Rating ?? 0,
                    Rank = codeforcesUserInfo?.Rank ?? "Unknown",
                    TitlePhoto = codeforcesUserInfo?.TitlePhoto ?? "default-avatar.png",
                    Handle = codeforcesUserInfo.Handle,
                    Id=newUser.Id,
                };
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
                    throw new Exception("Invalid email or email not confirmed.");

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (!result.Succeeded)
                    throw new Exception("Invalid email or password.");

                var codeforcesUserInfo = await _codeforcesService.GetUserInfoAsync(user.CodeForcesHandel);

                return new UserDto
                {
                    DisplayName = user.FullName,
                    Email = user.Email,
                    Token = await _tokenServices.CreateTokenAsync(user, _userManager),
                    Rating = codeforcesUserInfo?.Rating ?? 0,
                    Rank = codeforcesUserInfo?.Rank ?? "Unknown",
                    TitlePhoto = codeforcesUserInfo?.TitlePhoto ?? "default-avatar.png",
                    Handle = codeforcesUserInfo.Handle?? "Unknown",
                    Id = user.Id,
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in LoginAsync: {ex.Message}");
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in LogoutAsync: {ex.Message}");
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
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result;
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
    }
}
