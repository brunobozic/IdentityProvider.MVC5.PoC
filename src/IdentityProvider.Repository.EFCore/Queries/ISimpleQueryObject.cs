using System.Collections.Generic;

namespace IdentityProvider.Repository.EFCore.Queries
{
    public interface ISimpleQueryObject<T>
    {
        IEnumerable<T> Execute();
    }
}