
using IdentityProvider.Repository.EFCore.Domain;
using Module.CrossCutting.Models.ViewModels;
using URF.Core.Abstractions.Services;

namespace IdentityProvider.ServiceLayer.Services.AuditTrailService
{
    public interface IAuditTrailService : IService<DbAuditTrail>
    {
        List<YourCustomSearchClass> GetDataFromDbase(
            int userId
            , string searchBy
            , string searchValueExtra
            , string searchValueUserName
            , string searchValueOldValue
            , string searchValueNewValue
            , List<string> searchByTableNames
            , List<string> searchByActionNames
            , int take
            , int skip
            , string sortBy
            , bool sortDir
            , out int filteredResultsCount
            , out int totalResultsCount
        );
    }
}