using IdentityProvider.Models;
using IdentityProvider.Models.Domain.Account;
using Module.ServicePattern;
using System.Collections.Generic;
using IdentityProvider.Models.ViewModels.Operations;

namespace IdentityProvider.Services.OperationsService
{
    public interface IOperationService : IService<Operation>
    {
        IList<OperationsDatatableSearchClass> GetDataFromDbase(
            int userId
            , string searchBy
            , int take
            , int skip
            , string sortBy
            , bool sortDir
            , System.DateTime? from
            , System.DateTime? to
            , bool also_active
            , bool also_deleted
            , out int filteredResultsCount
            , out int totalResultsCount
        );
    }
}
