using IdentityProvider.Repository.EFCore.Domain.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityProvider.Repository.EFCore.Repositories
{
    public class ApplicationUserRepository : UserManager<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(
            IUserStore<ApplicationUser> store
            , IOptions<IdentityOptions> optionsAccessor
            , IPasswordHasher<ApplicationUser> passwordHasher
            , IEnumerable<IUserValidator<ApplicationUser>> userValidators
            , IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators
            , ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors
            , IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger
            , IHttpContextAccessor contextAccessor
            )
            : base(
                  store
                  , optionsAccessor
                  , passwordHasher
                  , userValidators
                  , passwordValidators
                  , keyNormalizer
                  , errors
                  , services
                  , logger
                  )
        { }

        public Task<ApplicationUser> FindByIdAsync(Guid userId)
        {
            return Users
                  .Include(c => c.Employee)
                .ThenInclude(c => c.OrganizationalUnits)
                .ThenInclude(c => c.OrganizationalUnit)
                .ThenInclude(c => c.Roles)
                .ThenInclude(c => c.Permissions)
                .ThenInclude(c => c.Permission)
                .ThenInclude(c => c.Operation)
                .Include(c => c.Employee)
                .ThenInclude(c => c.OrganizationalUnits)
                .ThenInclude(c => c.OrganizationalUnit)
                .ThenInclude(c => c.Roles)
                .ThenInclude(c => c.Permissions)
                .ThenInclude(c => c.Permission)
                .ThenInclude(c => c.Resource)
                .Include(c => c.UserProfile)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public override Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return Users
                 .Include(c => c.Employee)
                .ThenInclude(c => c.OrganizationalUnits)
                .ThenInclude(c => c.OrganizationalUnit)
                .ThenInclude(c => c.Roles)
                .ThenInclude(c => c.Permissions)
                .ThenInclude(c => c.Permission)
                .ThenInclude(c => c.Operation)
                .Include(c => c.Employee)
                .ThenInclude(c => c.OrganizationalUnits)
                .ThenInclude(c => c.OrganizationalUnit)
                .ThenInclude(c => c.Roles)
                .ThenInclude(c => c.Permissions)
                .ThenInclude(c => c.Permission)
                .ThenInclude(c => c.Resource)
                .Include(c => c.UserProfile)
                .FirstOrDefaultAsync(u => u.NormalizedUserName == userName.Trim().ToUpper());
        }

        public override Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return Users
                .Include(c => c.Employee)
                .ThenInclude(c => c.OrganizationalUnits)
                .ThenInclude(c => c.OrganizationalUnit)
                .ThenInclude(c => c.Roles)
                .ThenInclude(c => c.Permissions)
                .ThenInclude(c => c.Permission)
                .ThenInclude(c => c.Operation)
                .Include(c => c.Employee)
                .ThenInclude(c => c.OrganizationalUnits)
                .ThenInclude(c => c.OrganizationalUnit)
                .ThenInclude(c => c.Roles)
                .ThenInclude(c => c.Permissions)
                .ThenInclude(c => c.Permission)
                .ThenInclude(c => c.Resource)
                .Include(c => c.UserProfile)
                .FirstOrDefaultAsync(u => u.NormalizedEmail == email.Trim().ToUpper());
        }

        public override Task<ApplicationUser> FindByLoginAsync(string loginProvider, string providerKey)
        {
            return base.FindByLoginAsync(loginProvider, providerKey);
        }

        public override Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return base.GetRolesAsync(user);
        }

        public override Task<bool> IsInRoleAsync(ApplicationUser user, string role)
        {
            return base.IsInRoleAsync(user, role);
        }
    }
}