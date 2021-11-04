using System;
using System.Collections.Generic;
using IdentityProvider.Models.Domain.Account;
using IdentityProvider.Models.ViewModels.Operations;

using URF.Core.Abstractions.Services;

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
            , DateTime? from
            , DateTime? to
            , bool also_active
            , bool also_deleted
            , out int filteredResultsCount
            , out int totalResultsCount
        );
    }
}