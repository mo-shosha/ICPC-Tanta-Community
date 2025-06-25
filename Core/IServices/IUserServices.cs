using Core.DTO.AccountDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IUserServices
    {
        Task<string> ToggleBlockAsync(string UserId);
        Task<IEnumerable<Userinfo>> GetAllUsers();
        Task<IEnumerable<UserRatingDto>> GetAllUsersWithRating();
        Task<IEnumerable<UserRatingDto>> GetAllInstructorWithRating();
        Task<IEnumerable<Userinfo>> GetAllInstructors();
        int GetUserRanking(string userId, IEnumerable<UserRatingDto> sortedusers);
    }
}
