using System;

namespace Logging.WCF.Infrastructure.DomainCoreInterfaces
{
    public interface IModificationAuditedEntity
    {
        DateTimeOffset? DateModified { get; set; }
        long ModifiedBy { get; set; }
    }
}