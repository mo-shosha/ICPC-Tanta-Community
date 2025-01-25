using Core.DTO.AccountDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface ICodeforcesService
    {
        Task<CodeforcesUserInfo> GetUserInfoAsync(string handle);
    }
}
