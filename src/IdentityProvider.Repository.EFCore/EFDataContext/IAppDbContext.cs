using System.Threading;
using System.Threading.Tasks;

namespace IdentityProvider.Repository.EFCore.EFDataContext
{
    public interface IAppDbContext
    {
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}