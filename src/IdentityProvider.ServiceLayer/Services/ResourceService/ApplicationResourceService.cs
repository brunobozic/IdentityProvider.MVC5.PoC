
using IdentityProvider.Repository.EFCore.Domain.ResourceOperations;
using StructureMap;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;


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