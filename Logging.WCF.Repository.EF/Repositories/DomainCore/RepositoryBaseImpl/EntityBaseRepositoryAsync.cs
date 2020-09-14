using Logging.WCF.Repository.EF.DomainCoreInterfaces;
using Logging.WCF.Repository.EF.EFDataContext;
using Logging.WCF.Repository.EF.Repositories.DomainCore.RepositoryBaseImpl.RepositoryBaseInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Logging.WCF.Repository.EF.Repositories.DomainCore.RepositoryBaseImpl
{
    public class EntityBaseRepositoryAsync<T> : IEntityBaseRepositoryAsync<T>
        where T : class, IEntityBase, new()
    {
        private readonly MyAppDbContext _context;

        #region Properties

        public EntityBaseRepositoryAsync(MyAppDbContext context)
        {
            _context = context;
        }

        #endregion Properties

        public virtual IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsEnumerable();
        }

        public virtual Task<int> CountAsync()
        {
            return _context.Set<T>().CountAsync();
        }

        public virtual IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return query.AsEnumerable();
        }

        public async Task<T> GetSingleAsync(int id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T> GetSingleAsync(long id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return await query.Where(predicate).FirstOrDefaultAsync();
        }

        public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public virtual async void AddLogEntry(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry(entity);
            await _context.Set<T>().AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public virtual void DeleteWhere(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> entities = _context.Set<T>().Where(predicate);

            foreach (var entity in entities) _context.Entry(entity).State = EntityState.Deleted;
        }

        public virtual async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}