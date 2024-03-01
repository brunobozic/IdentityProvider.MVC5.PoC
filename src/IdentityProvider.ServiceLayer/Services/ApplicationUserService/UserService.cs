using IdentityProvider.Repository.EFCore.Domain.Account;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace IdentityProvider.ServiceLayer.Services.ApplicationUserService
{
    public class UserService : Service<ApplicationUser>, IUserService
    {
        public UserService(ITrackableRepository<ApplicationUser> repository) : base(repository)
        {
        }
    }
}
