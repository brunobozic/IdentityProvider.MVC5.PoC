using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvider.ServiceLayer.Services.RoleManagerService
{
    public class RoleManagerService : IRoleManagerService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagerService(RoleManager<IdentityRole> roleMgr)
        {
            _roleManager = roleMgr;
        }

        public RoleManager<IdentityRole> GetRoleManager()
        {
            return _roleManager;
        }

        public async Task<IdentityRole> GetRoleByNameAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            return role;
        }

        public async Task<List<IdentityRole>> GetRoles()
        {
            List<IdentityRole> roles = await _roleManager.Roles.ToListAsync();

            return roles;
        }

        public async Task<IdentityResult> AddRole(IdentityRole role)
        {
            var createResult = await _roleManager.CreateAsync(role);

            return createResult;
        }

        public async Task<IdentityResult> RemoveRole(IdentityRole role)
        {
            var createResult = await _roleManager.DeleteAsync(role);

            return createResult;
        }
    }
}