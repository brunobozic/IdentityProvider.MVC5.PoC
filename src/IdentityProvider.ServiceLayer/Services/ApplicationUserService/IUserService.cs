using IdentityProvider.Repository.EFCore.Domain.Account;
using URF.Core.Abstractions.Services;

namespace IdentityProvider.ServiceLayer.Services.ApplicationUserService
{
    public interface IUserService : IService<ApplicationUser>
    {
    }
}