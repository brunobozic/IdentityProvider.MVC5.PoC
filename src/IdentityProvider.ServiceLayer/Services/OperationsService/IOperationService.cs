using IdentityProvider.Repository.EFCore.Domain.ResourceOperations;
using Module.CrossCutting.Models.ViewModels.Operations;
using URF.Core.Abstractions.Services;

namespace IdentityProvider.ServiceLayer.Services.OperationsService
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