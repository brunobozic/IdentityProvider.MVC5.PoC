using System;

namespace Logging.WCF.Repository.EF.DomainCoreInterfaces
{
    public interface IDeactivatableEntity
    {
        bool IsActive { get; set; }
        DateTimeOffset ActiveFrom { get; set; }
        DateTimeOffset? ActiveTo { get; set; }
    }
}