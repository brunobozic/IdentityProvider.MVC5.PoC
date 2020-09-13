using Logging.WCF.Models.DTOs;

namespace Logging.WCF.Models
{
    public class LogToWCFServiceRequest
    {
        public LoggingEventDto LoggingEventDto { get; set; }
    }
}