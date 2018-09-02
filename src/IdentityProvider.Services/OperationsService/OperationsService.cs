using IdentityProvider.Models.Domain.Account;
using Module.Repository.EF.Repositories;
using Module.ServicePattern;

namespace IdentityProvider.Services.OperationsService
{
    public class OperationsService : Service<Operation>, IOperationService
    {
        [StructureMap.DefaultConstructor] // Set Default Constructor for StructureMap
        public OperationsService(IRepositoryAsync<Operation> repository) : base(repository)
        {
        }
    }
}