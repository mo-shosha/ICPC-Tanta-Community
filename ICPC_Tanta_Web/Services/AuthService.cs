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
                return null;

            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
                return null;

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var confirmationLink = $"https://localhost:7006/api/Auth/ConfirmEmail?userId={newUser.Id}&token={Uri.EscapeDataString(token)}";

            await _emailService.SendEmailAsync(newUser.Email, "Confirm Your Email", $"Please confirm your email by clicking <a href='{confirmationLink}'>here</a>.");

            return new UserDto
            {
                DisplayName = newUser.FullName,
                Email = newUser.Email,
                Token = await _tokenServices.CreateTokenAsync(newUser, _userManager),
                Rating = codeforcesUserInfo?.Rating ?? 0,
                Rank = codeforcesUserInfo?.Rank ?? "Unknown",
                Avatar = codeforcesUserInfo?.Avatar ?? "default-avatar.png",
                Handle = codeforcesUserInfo.Handle
            };
        }

        public async Task<UserDto> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                return null;

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
                return null;

            var codeforcesUserInfo = await _codeforcesService.GetUserInfoAsync(user.CodeForcesHandel);

            return new UserDto
            {
                DisplayName = user.FullName,
                Email = user.Email,
                Token = await _tokenServices.CreateTokenAsync(user, _userManager),
                Rating = codeforcesUserInfo?.Rating ?? 0,
                Rank = codeforcesUserInfo?.Rank ?? "Unknown",
                Avatar = codeforcesUserInfo?.Avatar ?? "default-avatar.png",
                Handle = codeforcesUserInfo.Handle
            };
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
        
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<UserDto> GetCurrentUserAsync(ClaimsPrincipal userClaims)
        {
            var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return null;

            var user = await _userManager.FindByNameAsync(userId);
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
                Avatar = codeforcesUserInfo?.Avatar ?? "default-avatar.png",
                Handle = codeforcesUserInfo.Handle
            };

        }

        public async Task<IdentityResult> AssignRoleToUserAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
                return IdentityResult.Failed(new IdentityError { Description = "Role does not exist" });

            return await _userManager.AddToRoleAsync(user, role);
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
    }
}
