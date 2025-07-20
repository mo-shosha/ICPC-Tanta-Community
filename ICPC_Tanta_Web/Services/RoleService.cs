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


        public async Task<IdentityResult> AssignRolesToUserAsync(List<string> roleNames, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            var existingRoles = await _userManager.GetRolesAsync(user);
            foreach (var roleName in existingRoles)
            {
                await _userManager.RemoveFromRoleAsync(user, roleName);
            }

            foreach (var roleName in roleNames)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                    return IdentityResult.Failed(new IdentityError { Description = $"Role '{roleName}' does not exist." });

                if (!await _userManager.IsInRoleAsync(user, roleName))
                {
                    var result = await _userManager.AddToRoleAsync(user, roleName);
                    if (!result.Succeeded)
                        return result;
                }
            }

            return IdentityResult.Success;
        }


        public async Task<IdentityResult> RemoveRoleFromUserAsync(string userId, string roleName, string defaultRole = "User")
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
                return IdentityResult.Failed(new IdentityError { Description = "Role does not exist." });

            var isInRole = await _userManager.IsInRoleAsync(user, roleName);
            if (!isInRole)
                return IdentityResult.Failed(new IdentityError { Description = "User is not in the specified role." });

            // Remove the role
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
                return result;

            // Check if user has any roles left
            var remainingRoles = await _userManager.GetRolesAsync(user);
            if (!remainingRoles.Any())
            {
                var defaultRoleExists = await _roleManager.RoleExistsAsync(defaultRole);
                if (!defaultRoleExists)
                {
                    // Create default role if it doesn't exist
                    await _roleManager.CreateAsync(new IdentityRole(defaultRole));
                }

                // Add default role
                return await _userManager.AddToRoleAsync(user, defaultRole);
            }

            return IdentityResult.Success;
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
