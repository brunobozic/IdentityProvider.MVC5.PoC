﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Module.Repository.EF.Repositories;
using TrackableEntities;

namespace Module.ServicePattern
{
    public abstract class Service<TEntity> : IService<TEntity> where TEntity : class, ITrackable
    {
        protected readonly IRepositoryAsync<TEntity> _repository;

        protected Service(IRepositoryAsync<TEntity> repository)
        {
            _repository = repository;
        }

        public virtual TEntity Find(params object[] keyValues)
        {
            return _repository.Find(keyValues);
        }

        public virtual IQueryable<TEntity> SelectQuery(string query, params object[] parameters)
        {
            return _repository.SelectQuery(query, parameters).AsQueryable();
        }

        public virtual void Insert(TEntity entity)
        {
            _repository.Insert(entity);
        }

        public virtual void ApplyChanges(TEntity entity)
        {
            _repository.ApplyChanges(entity);
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            _repository.InsertRange(entities);
        }

        [Obsolete(
            "InsertOrUpdateGraph has been deprecated.  Instead set TrackingState to Added or Modified and call ApplyChanges.")]
        public virtual void InsertOrUpdateGraph(TEntity entity)
        {
            _repository.InsertOrUpdateGraph(entity);
        }

        [Obsolete(
            "InsertOrUpdateGraph has been deprecated.  Instead set TrackingState to Added or Modified and call ApplyChanges.")]
        public virtual void InsertGraphRange(IEnumerable<TEntity> entities)
        {
            _repository.InsertGraphRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            _repository.Update(entity);
        }

        public virtual void Delete(object id)
        {
            _repository.Delete(id);
        }

        public virtual void Delete(TEntity entity)
        {
            _repository.Delete(entity);
        }

        public IQueryFluent<TEntity> Query()
        {
            return _repository.Query();
        }

        public virtual IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject)
        {
            return _repository.Query(queryObject);
        }

        public virtual IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query)
        {
            return _repository.Query(query);
        }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await _repository.FindAsync(keyValues);
        }

        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await _repository.FindAsync(cancellationToken, keyValues);
        }

        public virtual async Task<bool> DeleteAsync(params object[] keyValues)
        {
            return await DeleteAsync(CancellationToken.None, keyValues);
        }

        public virtual async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await _repository.DeleteAsync(cancellationToken, keyValues);
        }

        public async Task<bool> DeleteAsyncSoftDeletedAsync(CancellationToken cancellationToken, bool softDeleted, params object[] keyValues)
        {
            return await _repository.DeleteAsync(cancellationToken , true,keyValues );
        }

        public IQueryable<TEntity> Queryable()
        {
            return _repository.Queryable();
        }
    }
}