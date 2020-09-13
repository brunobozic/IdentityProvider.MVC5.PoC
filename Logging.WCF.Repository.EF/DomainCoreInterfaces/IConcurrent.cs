using System.ComponentModel.DataAnnotations;

namespace Logging.WCF.Repository.EF.DomainCoreInterfaces
{
    public interface IConcurrent
    {
        [Timestamp] byte[] RowVersion { get; set; }
    }
}