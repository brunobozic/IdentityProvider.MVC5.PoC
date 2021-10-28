using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Module.Repository.EF.Core3.BaseDomainModel;
using Module.Repository.EF.Core3.Contracts;

namespace Module.Repository.EF.Core3.Implementations
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public Repository(DbContext context)
        {
            Context = context;
            Set = context.Set<TEntity>();
        }

        protected DbContext Context { get; }
        protected DbSet<TEntity> Set { get; }

        public virtual async Task<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken = default)
        {
            return await Set.FindAsync(keyValues, cancellationToken);
        }

        public virtual async Task<TEntity> FindAsync<TKey>(TKey keyValue, CancellationToken cancellationToken = default)
        {
            return await FindAsync(new object[] {keyValue}, cancellationToken);
        }

        public virtual async Task<bool> ExistsAsync(object[] keyValues, CancellationToken cancellationToken = default)
        {
            var item = await FindAsync(keyValues, cancellationToken);
            return item != null;
        }

        public virtual async Task<bool> ExistsAsync<TKey>(TKey keyValue, CancellationToken cancellationToken = default)
        {
            return await ExistsAsync(new object[] {keyValue}, cancellationToken);
        }

        public virtual async Task LoadPropertyAsync(TEntity item, Expression<Func<TEntity, object>> property,
            CancellationToken cancellationToken = default)
        {
            await Context.Entry(item).Reference(property).LoadAsync(cancellationToken);
        }

        public virtual void Attach(TEntity item)
        {
            Set.Attach(item);
        }

        public virtual void Detach(TEntity item)
        {
            Context.Entry(item).State = EntityState.Detached;
        }

        public virtual void Insert(TEntity item)
        {
            Context.Entry(item).State = EntityState.Added;
        }

        public virtual void Update(TEntity item)
        {
            Context.Entry(item).State = EntityState.Modified;
        }

        public virtual void Delete(TEntity item)
        {
            Context.Entry(item).State = EntityState.Deleted;
        }

        public virtual async Task<bool> DeleteAsync(object[] keyValues, CancellationToken cancellationToken = default)
        {
            var item = await FindAsync(keyValues, cancellationToken);
            if (item == null) return false;
            Context.Entry(item).State = EntityState.Deleted;
            return true;
        }

        public virtual async Task<bool> DeleteAsync<TKey>(TKey keyValue, CancellationToken cancellationToken = default)
        {
            return await DeleteAsync(new object[] {keyValue}, cancellationToken);
        }

        public virtual IQueryable<TEntity> Queryable()
        {
            return Set;
        }

        public virtual IQueryable<TEntity> QueryableSql(string sql, params object[] parameters)
        {
            return Set.FromSqlRaw(sql, parameters);
        }

        public virtual IQuery<TEntity> Query()
        {
            return new Query<TEntity>(this);
        }
    }
}