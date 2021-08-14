using System;

namespace Logging.WCF.Infrastructure.DomainCoreInterfaces
{
    public interface ISoftDeletable
    {
        bool Deleted { get; set; }
        DateTimeOffset? DateDeleted { get; set; }
        long DeletedBy { get; set; }
    }
}