using EFModule.Core.Abstractions.Trackable;
using EFModule.Core.Services;
using IdentityProvider.Repository.EFCore.Domain.ResourceOperations;
using StructureMap;

namespace IdentityProvider.ServiceLayer.Services.ResourceService
{
    public class ApplicationResourceService : Service<Resource>
    {
        [DefaultConstructor] // Set Default Constructor for StructureMap
        public ApplicationResourceService(ITrackableRepository<Resource> repository) : base(repository)
        {
        }
    }
}