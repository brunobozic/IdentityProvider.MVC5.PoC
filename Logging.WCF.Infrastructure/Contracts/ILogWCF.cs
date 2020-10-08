using Logging.WCF.Models;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Logging.WCF.Infrastructure.Contracts
{
    [ServiceContract]
    public interface ILogWcf
    {
        [OperationContract]
        Task LogToWcfAsync(LogToWCFServiceRequest request);
        void Dispose();
    }
}