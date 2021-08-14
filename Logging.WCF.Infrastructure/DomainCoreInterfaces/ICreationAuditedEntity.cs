using System;

namespace Logging.WCF.Infrastructure.DomainCoreInterfaces
{
    public interface ICreationAuditedEntity
    {
        DateTimeOffset DateCreated { get; set; }

        long CreatedBy { get; set; }
    }
}