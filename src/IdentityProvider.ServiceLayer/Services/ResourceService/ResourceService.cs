
using IdentityProvider.Repository.EFCore.Domain.ResourceOperations;
using StructureMap;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;


namespace IdentityProvider.ServiceLayer.Services.ResourceService
{
    public class ResourceService : Service<Resource>
    {
        [DefaultConstructor] // Set Default Constructor for StructureMap
        public ResourceService(ITrackableRepository<Resource> repository) : base(repository)
        {
        }
    }
}