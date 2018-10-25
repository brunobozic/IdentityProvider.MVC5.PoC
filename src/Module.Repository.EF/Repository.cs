using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using IdentityProvider.Infrastructure.Domain;
using LinqKit;
using Module.Repository.EF.Repositories;
using Module.Repository.EF.RowLevelSecurity;
using Module.Repository.EF.UnitOfWorkInterfaces;
using TrackableEntities;
using TrackableEntities.EF6;

namespace Module.Repository.EF
{
    /// <inheritdoc />
    public class Repository<TEntity> : IRepositoryAsync<TEntity> where TEntity : class, ITrackable, ISoftDeletable
    {
        protected readonly DbContext Context;
        protected readonly DbSet<TEntity> Set;
        protected readonly IQueryable<TEntity> _innerSet;
        protected readonly IUnitOfWorkAsync UnitOfWork;
        private readonly IRowAuthPoliciesContainer _container;
        private readonly Expression<Func<TEntity , bool>> authFilter;

        public Repository( 
            DbContext context 
            , IUnitOfWorkAsync unitOfWork 
            , IRowAuthPoliciesContainer rowAuthPoliciesContainer 
        )
        {
            UnitOfWork = unitOfWork;
            Context = context;
            Set = context.Set<TEntity>();
            _container = rowAuthPoliciesContainer;
            _innerSet = context.Set<TEntity>();
            authFilter = BuildWhereExpression<TEntity>();
            _innerSet = _innerSet.Where(authFilter);
        }

        private Expression<Func<T , bool>> BuildWhereExpression<T>()
        {
            if (_container.HasPolicy<T>())
            {
                var policy = _container.GetPolicy<T>();
                return policy.BuildAuthFilterExpression();
            }
            else
            {
                Expression<Func<T , bool>> trueExpression = entity => true;
                return trueExpression;
            }
        }

        public virtual TEntity Find( params object[] keyValues )
        {
            return Set.Find(keyValues);
        }

        public virtual TEntity FindWithRowLevelSecurity( params object[] keyValues )
        {
            var isString = keyValues[ 0 ] as string;
            var isInt = keyValues[ 0 ] is int i1 ? i1 : -1;


            // var entityFound = isInt > 0 ? _innerSet.SingleOrDefault(i => i.Id.Equals(isInt)) : _innerSet.SingleOrDefault(i => i.Id.Equals(isString));
            // var entityFound = _innerSet.SingleOrDefault(i => i.Id == isInt);
            // return entityFound;

            return null;
        }

        public virtual IQueryable<TEntity> SelectQuery( string query , params object[] parameters )
        {
            return Set.SqlQuery(query , parameters).AsQueryable();
        }

        public virtual void Insert( TEntity entity , bool traverseGraph = true )
        {
            entity.TrackingState = TrackingState.Added;

            if (traverseGraph)
                Context.ApplyChanges(entity);
            else
                Context.Entry(entity).State = EntityState.Added;
        }

        public void ApplyChanges( TEntity entity )
        {
            Context.ApplyChanges(entity);
        }

        public virtual void InsertRange( IEnumerable<TEntity> entities , bool traverseGraph = true )
        {
            foreach (var entity in entities)
            {
                Insert(entity , traverseGraph);
            }
        }

        [Obsolete(
            "InsertGraphRange has been deprecated. Instead call Insert to set TrackingState on enttites in a graph.")]
        public virtual void InsertGraphRange( IEnumerable<TEntity> entities )
        {
            InsertRange(entities);
        }

        public virtual void Update( TEntity entity , bool traverseGraph = true )
        {
            entity.TrackingState = TrackingState.Modified;

            if (traverseGraph)
                Context.ApplyChanges(entity);
            else
                Context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete( params object[] keyValues )
        {
            var entity = Set.Find(keyValues);
            Delete(entity);
        }

        public void DeleteWithRowLevelSecurity( params object[] keyValues )
        {
            var entity = FindWithRowLevelSecurity(keyValues);
            Delete(entity);
        }

        public virtual void Delete( TEntity entity )
        {
            entity.TrackingState = TrackingState.Deleted;
            Context.ApplyChanges(entity);
        }

        public IQueryFluent<TEntity> Query()
        {
            return new QueryFluent<TEntity>(this);
        }

        public virtual IQueryFluent<TEntity> Query( IQueryObject<TEntity> queryObject )
        {
            return new QueryFluent<TEntity>(this , queryObject);
        }

        public virtual IQueryFluent<TEntity> Query( Expression<Func<TEntity , bool>> query )
        {
            return new QueryFluent<TEntity>(this , query);
        }

        public IQueryable<TEntity> Queryable()
        {
            return Set;
        }

        public IQueryable<TEntity> QueryableWithRowLevelSecurity()
        {
            return _innerSet;
        }

        public IRepository<T> GetRepository<T>() where T : class, ITrackable
        {
            return UnitOfWork.Repository<T>();
        }

        public virtual async Task<TEntity> FindAsync( params object[] keyValues )
        {
            return await Set.FindAsync(keyValues);
        }

        public virtual async Task<TEntity> FindAsync( CancellationToken cancellationToken , params object[] keyValues )
        {
            return await Set.FindAsync(cancellationToken , keyValues);
        }

        public virtual async Task<bool> DeleteAsync( params object[] keyValues )
        {
            if (await DeleteAsync(CancellationToken.None , keyValues)) return true;
            return false;
        }
        public virtual async Task<bool> DeleteAsyncSoftDeleted( bool softDeleted = true , params object[] keyValues )
        {
            //int kv = ( int ) keyValues[ 0 ];
            //var entity = await Queryable().Where(i => i.IsDeleted == false && i.Id.Equals(kv)).SingleAsync();

            //if (entity == null)
            //    return false;

            //entity.TrackingState = TrackingState.Deleted;
            //Context.ApplyChanges(entity);
            return true;
        }
        public virtual async Task<bool> DeleteAsync( CancellationToken cancellationToken , params object[] keyValues )
        {
            var entity = await FindAsync(cancellationToken , keyValues);

            if (entity == null)
                return false;

            entity.TrackingState = TrackingState.Deleted;
            Context.ApplyChanges(entity);

            return true;
        }

        public virtual async Task<IEnumerable<TEntity>> SelectQueryAsync( string query , params object[] parameters )
        {
            return await Set.SqlQuery(query , parameters).ToArrayAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> SelectQueryAsync( string query ,
            CancellationToken cancellationToken , params object[] parameters )
        {
            return await Set.SqlQuery(query , parameters).ToArrayAsync(cancellationToken);
        }

        [Obsolete(
            "InsertOrUpdateGraph has been deprecated.  Instead set TrackingState to Added or Modified and call ApplyChanges.")]
        public virtual void InsertOrUpdateGraph( TEntity entity )
        {
            ApplyChanges(entity);
        }

        internal IQueryable<TEntity> Select(
            Expression<Func<TEntity , bool>> filter = null ,
            Func<IQueryable<TEntity> , IOrderedQueryable<TEntity>> orderBy = null ,
            List<Expression<Func<TEntity , object>>> includes = null ,
            int? page = null ,
            int? pageSize = null )
        {
            IQueryable<TEntity> query = Set;

            if (includes != null)
                query = includes.Aggregate(query , ( current , include ) => current.Include(include));
            if (orderBy != null)
                query = orderBy(query);
            if (filter != null)
                query = query.AsExpandable().Where(filter);
            if (page != null && pageSize != null)
                query = query.Skip(( page.Value - 1 ) * pageSize.Value).Take(pageSize.Value);
            return query;
        }

        internal async Task<IEnumerable<TEntity>> SelectAsync(
            Expression<Func<TEntity , bool>> filter = null ,
            Func<IQueryable<TEntity> , IOrderedQueryable<TEntity>> orderBy = null ,
            List<Expression<Func<TEntity , object>>> includes = null ,
            int? page = null ,
            int? pageSize = null )
        {
            return await Select(filter , orderBy , includes , page , pageSize).ToListAsync();
        }
    }
}