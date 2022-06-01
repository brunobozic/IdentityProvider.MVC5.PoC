using EFModule.Core.Abstractions.Trackable;
using TrackableEntities.Common.Core;

namespace EFModule.Core.Abstractions.Services
{
    public interface IService<TEntity> : ITrackableRepository<TEntity> where TEntity : class, ITrackable
    {
    }
}