using System.Collections.Generic;

namespace IdentityProvider.Repository.EF.Queries
{
    public interface ISimpleQueryObject<T>
    {
        IEnumerable<T> Execute();
    }
}