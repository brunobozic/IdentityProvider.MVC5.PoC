using System.Threading.Tasks;

namespace IdentityProvider.Repository.EFCore
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync();
    }
}