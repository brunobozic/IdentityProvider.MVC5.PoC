using System;

namespace Logging.WCF.Repository.EF.DomainCoreInterfaces
{
    public interface ISoftDeletable
    {
        bool Deleted { get; set; }
        DateTimeOffset? DateDeleted { get; set; }
        long DeletedBy { get; set; }
    }
}