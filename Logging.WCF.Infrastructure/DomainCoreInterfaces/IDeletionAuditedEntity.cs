using System;

namespace Logging.WCF.Infrastructure.DomainCoreInterfaces
{
    public interface IDeletionAuditedEntity
    {
        DateTimeOffset? DateDeleted { get; set; }
        long DeletedBy { get; set; }
    }
}