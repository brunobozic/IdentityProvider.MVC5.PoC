using IdentityProvider.Models.Domain.Account;
using IdentityProvider.Repository.EF.EFDataContext;
using Microsoft.AspNet.Identity;

namespace IdentityProvider.Services
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        private readonly AppDbContext _context;

        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> roleStore,
            AppDbContext context ) :
            base(roleStore)
        {
            _context = context;
        }

    }
}