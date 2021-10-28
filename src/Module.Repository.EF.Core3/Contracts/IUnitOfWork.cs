using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Module.Repository.EF.Core3.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<object> parameters,
            CancellationToken cancellationToken = default);
    }
}