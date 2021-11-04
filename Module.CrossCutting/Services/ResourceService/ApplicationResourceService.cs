using IdentityProvider.Models.Domain.Account;
using Module.Repository.EF.Repositories;
using Module.ServicePattern;
using StructureMap;

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