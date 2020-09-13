using Logging.WCF.Models;
using System.ServiceModel;

namespace Logging.WCF.Infrastructure.Contracts
{
    [ServiceContract]
    public interface ILogWcf
    {
        [OperationContract]
        void LogToWcf(LogToWCFServiceRequest request);
    }
}