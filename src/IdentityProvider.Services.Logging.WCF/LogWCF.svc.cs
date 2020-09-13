using HAC.Helpdesk.Services.Logging.WCF.StructureMap;
using Logging.WCF.Infrastructure.Contracts;
using Logging.WCF.Models;
using System.ServiceModel;

namespace HAC.Helpdesk.Services.Logging.WCF
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class LogWcfService : ILogWcf
    {
        private readonly ILogSinkerService _service;

        public LogWcfService(ILogSinkerService dbLogService)
        {
            _service = dbLogService;
        }

        public LogWcfService()
        {
            _service = Ioc.GetContainer().GetInstance<ILogSinkerService>();
        }

        public void LogToWcf(LogToWCFServiceRequest request)
        {
            _service.LogAsync(request.LoggingEventDto);
        }
    }
}