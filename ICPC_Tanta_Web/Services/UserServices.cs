using Core.DTO.AccountDTO;
using Core.Entities.Identity;
using Core.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ICPC_Tanta_Web.Services
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICodeforcesService _codeforcesService;
        public UserServices(UserManager<ApplicationUser> userManager, ICodeforcesService codeforcesService)
        {
            _userManager = userManager;
            _codeforcesService = codeforcesService;
        }

        public async Task<IEnumerable<Userinfo>> GetAllInstructors()
        {
            var users = await _userManager.Users.ToListAsync();
            var instructors = new List<Userinfo>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Instructor"))  
                {
                    instructors.Add(new Userinfo
                    {
                        Id = user.Id,
                        Name = user.UserName,
                        Email = user.Email,
                        Handle = user.CodeForcesHandel,
                        PhoneNumber = user.PhoneNumber
                    });
                }
            }

            return instructors;
        }

        public async Task<IEnumerable<Userinfo>> GetAllUsers()
        {
            var users = await _userManager.Users
                .Select(user => new Userinfo
                {
                    Id = user.Id,
                    Name = user.UserName,
                    Email = user.Email,
                    Handle = user.CodeForcesHandel,
                    PhoneNumber = user.PhoneNumber
                })
                .ToListAsync();

            return users;
        }

        public async Task<IEnumerable<UserRatingDto>> GetAllUsersWithRating()
        {
            var users = await _userManager.Users.ToListAsync();
            var SelectedUsers = new List<UserRatingDto>();

            foreach (var user in users)
            {
                var codeforcesUserInfo = await _codeforcesService.GetUserInfoAsync(user.CodeForcesHandel);
                var roles = await _userManager.GetRolesAsync(user);
                
                if (roles.Contains("User"))
                {
                    SelectedUsers.Add(new UserRatingDto
                    {
                        Id=user.Id,
                        Name = user.UserName,
                        Rating=codeforcesUserInfo.Rating,
                        ImgURL=codeforcesUserInfo.TitlePhoto,
                    });
                }
            }

            return SelectedUsers.OrderByDescending(u => u.Rating).ToList();
        }

        public int GetUserRanking(string userId, IEnumerable<UserRatingDto> sortedUsers)
        {
            var userRanking = sortedUsers.ToList().FindIndex(u => u.Id == userId) + 1;
            if (userRanking == 0)
            {
                return -1;  
            }

            return userRanking;
        }

    }
}
