using System;

namespace Logging.WCF.Infrastructure.DomainCoreInterfaces
{
    public interface IDeactivatableEntity
    {
        bool IsActive { get; set; }
        DateTimeOffset ActiveFrom { get; set; }
        DateTimeOffset? ActiveTo { get; set; }
    }
}