using System.ServiceModel;

namespace IdentityProvider.Infrastructure.DatabaseLog
{
    [ServiceContract]
    public interface ILogWcf
    {
        [OperationContract]
        void AppendToLog(LogToDatabaseRequest request);
    }
}