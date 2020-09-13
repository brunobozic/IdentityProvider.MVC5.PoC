using System;

namespace Logging.WCF.Repository.EF.DomainCoreInterfaces
{
    public interface ICreationAuditedEntity
    {
        DateTimeOffset DateCreated { get; set; }

        long CreatedBy { get; set; }
    }
}