using Module.Repository.EF.Repositories;
using System.Data;
using TrackableEntities;

namespace Module.Repository.EF.UnitOfWorkInterfaces
{
    public interface IUnitOfWork
    {
        int? CommandTimeout { get; set; }
        int SaveChanges();
        int ExecuteSqlCommand(string sql, params object[] parameters);
        IRepository<TEntity> Repository<TEntity>() where TEntity : class, ITrackable;
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
        bool Commit();
        void Rollback();
    }
}