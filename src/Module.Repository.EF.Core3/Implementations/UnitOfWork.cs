using Microsoft.EntityFrameworkCore;
using Module.Repository.EF.Core3.Contracts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Module.Repository.EF.Core3.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        protected DbContext Context { get; }

        public UnitOfWork(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await Context.SaveChangesAsync(cancellationToken);

        public virtual async Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default)
            => await Context.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
    }
}