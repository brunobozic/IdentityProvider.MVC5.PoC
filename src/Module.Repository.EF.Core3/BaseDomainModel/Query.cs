using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Module.Repository.EF.Core3.Contracts;

namespace Module.Repository.EF.Core3.BaseDomainModel
{
    public class Query<TEntity> : IQuery<TEntity> where TEntity : class
    {
        private IOrderedQueryable<TEntity> _orderedQuery;
        private IQueryable<TEntity> _query;
        private int? _skip;
        private int? _take;

        public Query(IRepository<TEntity> repository)
        {
            _query = repository.Queryable();
        }

        public virtual IQuery<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return Set(q => q._query = q._query.Where(predicate));
        }

        public virtual IQuery<TEntity> Include<TProperty>(Expression<Func<TEntity, TProperty>> navigationProperty)
        {
            return Set(q => q._query = q._query.Include(navigationProperty));
        }

        public virtual IQuery<TEntity> Include(string navigationPropertyPath)
        {
            return Set(q => q._query = q._query.Include(navigationPropertyPath));
        }

        public virtual IQuery<TEntity> OrderBy(Expression<Func<TEntity, object>> keySelector)
        {
            if (_orderedQuery == null) _orderedQuery = _query.OrderBy(keySelector);
            else _orderedQuery.OrderBy(keySelector);
            return this;
        }

        public virtual IQuery<TEntity> ThenBy(Expression<Func<TEntity, object>> thenBy)
        {
            return Set(q => q._orderedQuery.ThenBy(thenBy));
        }

        public virtual IQuery<TEntity> OrderByDescending(Expression<Func<TEntity, object>> keySelector)
        {
            if (_orderedQuery == null) _orderedQuery = _query.OrderByDescending(keySelector);
            else _orderedQuery.OrderByDescending(keySelector);
            return this;
        }

        public virtual IQuery<TEntity> ThenByDescending(Expression<Func<TEntity, object>> thenByDescending)
        {
            return Set(q => q._orderedQuery.ThenByDescending(thenByDescending));
        }

        public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _query.CountAsync(cancellationToken);
        }

        public virtual IQuery<TEntity> Skip(int skip)
        {
            return Set(q => q._skip = skip);
        }

        public virtual IQuery<TEntity> Take(int take)
        {
            return Set(q => q._take = take);
        }

        public virtual async Task<IEnumerable<TEntity>> SelectAsync(CancellationToken cancellationToken = default)
        {
            _query = _orderedQuery ?? _query;

            if (_skip.HasValue) _query = _query.Skip(_skip.Value);
            if (_take.HasValue) _query = _query.Take(_take.Value);

            return await _query.ToListAsync(cancellationToken);
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
        {
            return await _query.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _query.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public virtual async Task<TEntity> SingleOrDefaultAsync(CancellationToken cancellationToken = default)
        {
            return await _query.SingleOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _query.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _query.AnyAsync(predicate, cancellationToken);
        }

        public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _query.AnyAsync(cancellationToken);
        }

        public virtual async Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _query.AllAsync(predicate, cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> SelectSqlAsync(string sql, object[] parameters,
            CancellationToken cancellationToken = default)
        {
            return await (_query as DbSet<TEntity>)?.FromSqlRaw(sql, parameters).ToListAsync(cancellationToken);
        }

        private IQuery<TEntity> Set(Action<Query<TEntity>> setParameter)
        {
            setParameter(this);
            return this;
        }
    }
}