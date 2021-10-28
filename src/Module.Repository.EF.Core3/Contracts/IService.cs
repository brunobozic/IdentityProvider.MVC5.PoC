using TrackableEntities.Common.Core;

namespace Module.Repository.EF.Core3.Contracts
{
    public interface IService<TEntity> : ITrackableRepository<TEntity> where TEntity : class, ITrackable
    {
    }
}