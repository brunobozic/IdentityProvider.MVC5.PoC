using System;

namespace Logging.WCF.Repository.EF.DomainCoreInterfaces
{
    public interface IModificationAuditedEntity
    {
        DateTimeOffset? DateModified { get; set; }
        long ModifiedBy { get; set; }
    }
}