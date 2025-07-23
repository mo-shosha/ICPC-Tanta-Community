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
        //private readonly ICodeforcesService _codeforcesService;
        public UserServices(UserManager<ApplicationUser> userManager, ICodeforcesService codeforcesService)
        {
            _userManager = userManager;
           // _codeforcesService = codeforcesService;
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
                        Name = user.FullName,
                        Email = user.Email,
                        Handle = user.CodeForcesHandel,
                        PhoneNumber = user.PhoneNumber
                    });
                }
            }

            return instructors;
        }

        public async Task<IEnumerable<UserRatingDto>> GetAllInstructorWithRating()
        {
            var users = await _userManager.Users.ToListAsync();
            var SelectedInstructors = new List<UserRatingDto>();

            foreach (var user in users)
            {
                //var codeforcesUserInfo = await _codeforcesService.GetUserInfoAsync(user.CodeForcesHandel);
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Instructor"))
                {
                    SelectedInstructors.Add(new UserRatingDto
                    {
                        Id = user.Id,
                        Name = user.FullName,
                        Rating = user.Rating,
                        ImgURL = user.TitlePhoto,

                    });
                }
            }

            return SelectedInstructors.OrderByDescending(u => u.Rating).ToList();
        }

        public async Task<IEnumerable<CustemUserInfo>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            var result = new List<CustemUserInfo>();

            foreach (var user in users)
            {
                var isLocked = await _userManager.IsLockedOutAsync(user);
                //var codeforcesUserInfo = await _codeforcesService.GetUserInfoAsync(user.CodeForcesHandel);
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new CustemUserInfo
                {
                    Id = user.Id,
                    Name = user.FullName,
                    Email = user.Email,
                    Handle = user.CodeForcesHandel,
                    PhoneNumber = user.PhoneNumber,
                    IsLocked = isLocked,
                    Rank= user.Rank,
                    Rating=user.Rating,
                    Roles=roles.ToList()

                });
            }

            return result;
        }

        public async Task<IEnumerable<UserRatingDto>> GetAllUsersWithRating()
        {
            var users = await _userManager.Users.ToListAsync();
            var SelectedUsers = new List<UserRatingDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                
                if (roles.Contains("User")&&roles.Count()==1)
                {
                    SelectedUsers.Add(new UserRatingDto
                    {
                        Id=user.Id,
                        Name = user.FullName,
                        Rating=user.Rating,
                        ImgURL=user.TitlePhoto,
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

        public async Task<string> ToggleBlockAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
             
            if (user == null)
                return "User not found.";

            var isLockedOut = await _userManager.IsLockedOutAsync(user);
            if (isLockedOut)
            {
                await _userManager.SetLockoutEndDateAsync(user, null);
                return "User unblocked successfully.";
            }
            else
            {
                await _userManager.SetLockoutEnabledAsync(user, true);
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
                return "User blocked successfully.";
            }

        }
    }
}


//var usersWithOnlyUserRole = await (
//                from user in _context.Users
//                join userRole in _context.UserRoles on user.Id equals userRole.UserId
//                join role in _context.Roles on userRole.RoleId equals role.Id
//                group role.Name by new { user.Id, user.UserName, user.Email, user.Rating, user.TitlePhoto } into g
//                where g.Count() == 1 && g.First() == "User"
//                orderby g.Key.Rating descending
//                select new UserRatingDto
//                {
//                    Id = g.Key.Id,
//                    Name = g.Key.UserName,
//                    Rating = g.Key.Rating,
//                    ImgURL = g.Key.TitlePhoto
//                }
//            ).ToListAsync();

//return usersWithOnlyUserRole;