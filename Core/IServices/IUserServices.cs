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
        Task<IEnumerable<Userinfo>> GetAllUsers();
        Task<IEnumerable<UserRatingDto>> GetAllUsersWithRating();
        Task<IEnumerable<Userinfo>> GetAllInstructors();
        int GetUserRanking(string userId, IEnumerable<UserRatingDto> sortedusers);
    }
}
