using IdentityProvider.Models.Domain.Account;
using Microsoft.AspNet.Identity;
using Module.Repository.EF.SimpleAudit;

namespace IdentityProvider.Services
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        private readonly IAuditedDbContext<ApplicationUser> _context;

        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> roleStore,
            IAuditedDbContext<ApplicationUser> context) :
            base(roleStore)
        {
            _context = context;
        }

    }
}