using IdentityProvider.Repository.EFCore.Domain.Account;
using System.Threading.Tasks;

namespace IdentityProvider.Repository.EFCore
{
    public interface IApplicationUserRepository
    {
        Task<ApplicationUser> FindByIdAsync(string userId);
    }
}