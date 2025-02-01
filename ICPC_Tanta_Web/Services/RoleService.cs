using Core.DTO.AccountDTO;
using Core.Entities.Identity;
using Core.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ICPC_Tanta_Web.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IdentityResult> AddRoleAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
            {
                throw new Exception("Role already exists.");
            }

            var role = new IdentityRole(roleName);
            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> AssignRoleToUserAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
                return IdentityResult.Failed(new IdentityError { Description = "Role does not exist" });

            return await _userManager.AddToRoleAsync(user, roleName);
        }
     

        public async Task<IdentityResult> DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                throw new Exception("Role not found.");
            }

            return await _roleManager.DeleteAsync(role);
        }

        public IEnumerable<IdentityRole> GetAllRoles()
        {
            return _roleManager.Roles.ToList();
        }

        public async Task<IEnumerable<Userinfo>> GetAllUser()
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


    }
}
