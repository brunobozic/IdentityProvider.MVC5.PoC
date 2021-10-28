using System.ServiceModel;
using System.Threading.Tasks;
using Logging.WCF.Models;

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