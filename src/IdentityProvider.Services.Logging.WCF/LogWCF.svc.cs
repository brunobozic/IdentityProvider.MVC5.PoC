using HAC.Helpdesk.Services.Logging.WCF.StructureMap;
using Logging.WCF.Infrastructure.Contracts;
using Logging.WCF.Models;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace HAC.Helpdesk.Services.Logging.WCF
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class LogWcfService : ILogWcf, IDisposable
    {
        private readonly ILogSinkerService _logSinkerService;

        public LogWcfService(ILogSinkerService logSinker)
        {
            _logSinkerService = logSinker;
        }

        public LogWcfService()
        {
            _logSinkerService = Ioc.GetContainer().GetInstance<ILogSinkerService>();
        }

        public async Task LogToWcfAsync(LogToWCFServiceRequest request)
        {
           await _logSinkerService.SinkToLogAsync(request.LoggingEventDto);
        }

        public void Dispose()
        {
            if (_logSinkerService != null)
            {
                // only works if transient
                try
                {
                    Ioc.GetContainer().Release(_logSinkerService);
                }
                catch (Exception ex)
                {
                    // not interested
                }
                finally 
                {
                    //
                }
                
            }
        }
    }
}