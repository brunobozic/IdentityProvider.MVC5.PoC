using EFModule.Core.Abstractions.Services;
using IdentityProvider.Repository.EFCore.Domain.ResourceOperations;

namespace IdentityProvider.ServiceLayer.Services.ResourceService
{
    public interface IApplicationResourceService : IService<Resource>
    {
    }
}