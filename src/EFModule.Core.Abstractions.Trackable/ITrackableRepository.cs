using System.Threading.Tasks;
using TrackableEntities.Common.Core;

namespace EFModule.Core.Abstractions.Trackable
{
    public interface ITrackableRepository<TEntity> : IRepository<TEntity> where TEntity : class, ITrackable
    {
        void ApplyChanges(params TEntity[] entities);

        void AcceptChanges(params TEntity[] entities);

        void DetachEntities(params TEntity[] entities);

        Task LoadRelatedEntities(params TEntity[] entities);
    }
}