using System.ComponentModel.DataAnnotations;

namespace Logging.WCF.Infrastructure.DomainCoreInterfaces
{
    public interface IConcurrent
    {
        [Timestamp] byte[] RowVersion { get; set; }
    }
}