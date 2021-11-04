using IdentityProvider.Models.Domain.Account;
using StructureMap;
using URF.Core.Services;

namespace IdentityProvider.Services.ResourceService
{
    public class ApplicationResourceService : Service<ApplicationResource>, IApplicationResourceService
    {
        [DefaultConstructor] // Set Default Constructor for StructureMap
        public ApplicationResourceService(IRepositoryAsync<ApplicationResource> repository) : base(repository)
        {
        }
    }
}