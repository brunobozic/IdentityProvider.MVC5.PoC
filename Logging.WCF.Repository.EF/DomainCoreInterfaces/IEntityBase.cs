using System;

namespace Logging.WCF.Repository.EF.DomainCoreInterfaces
{
    public interface IEntityBase
    {
        long Id { get; set; }
    }
}