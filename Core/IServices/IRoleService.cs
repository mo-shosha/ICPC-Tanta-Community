using Core.DTO.AccountDTO;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IRoleService
    {
        Task<IdentityResult> AddRoleAsync(string roleName);
        Task<IdentityResult> DeleteRoleAsync(string roleName);
        IEnumerable<IdentityRole> GetAllRoles();
        //Task<IdentityResult> AssignRoleToUserAsync(string userId, string roleName);
        Task<IdentityResult> AssignRolesToUserAsync(List<string> roleNames, string userId);
        Task<IdentityResult> RemoveRoleFromUserAsync(string userId, string roleName, string defaultRole = "User");
        Task<IEnumerable<Userinfo>> GetAllUser();
    }
}
