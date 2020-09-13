using System;

namespace Logging.WCF.Repository.EF.DomainCoreInterfaces
{
    public interface IDeletionAuditedEntity
    {
        DateTimeOffset? DateDeleted { get; set; }
        long DeletedBy { get; set; }
    }
}