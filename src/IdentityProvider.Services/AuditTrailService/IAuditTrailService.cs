using System.Collections.Generic;
using IdentityProvider.Infrastructure.DatabaseAudit;
using IdentityProvider.Models;
using Module.ServicePattern;

namespace IdentityProvider.Services.AuditTrailService
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
            , int take
            , int skip
            , string sortBy
            , bool sortDir
            , out int filteredResultsCount
            , out int totalResultsCount
        );
    }
}
