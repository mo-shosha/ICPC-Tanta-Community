using Core.DTO.AccountDTO;
using Core.Entities.Identity;
using Core.IServices;
using Microsoft.AspNetCore.Identity;

namespace ICPC_Tanta_Web.Services
{
    public class AuthService: IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenServices _tokenServices;
        private readonly ICodeforcesService _codeforcesService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenServices tokenServices,
            ICodeforcesService codeforcesService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenServices = tokenServices;
            _codeforcesService = codeforcesService;
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

            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
                return null;

            var codeforcesUserInfo = await _codeforcesService.GetUserInfoAsync(model.CodeForcesHandel);

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
            if (user == null)
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
    }
}
