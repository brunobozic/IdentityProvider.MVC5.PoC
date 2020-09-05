using IdentityProvider.Models.Domain.Account;
using Module.Repository.EF.Repositories;
using Module.ServicePattern;

namespace IdentityProvider.Services.ResourceService
{
    public class ApplicationResourceService : Service<ApplicationResource>, IApplicationResourceService
    {
        [StructureMap.DefaultConstructor] // Set Default Constructor for StructureMap
        public ApplicationResourceService(IRepositoryAsync<ApplicationResource> repository) : base(repository)
        {
        }
    }
}