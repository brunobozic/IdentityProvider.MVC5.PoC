using log4net.Core;
using log4net.Repository;
using log4net.Util;
using System;

namespace IdentityProvider.Infrastructure.DatabaseLog.DTOs
{
    public class LoggingEventDto
    {
        public string DisplayName { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
        public DateTime TimeStamp { get; set; }
        public string ThreadName { get; set; }
        public ILoggerRepository Repository { get; set; }
        public string RenderedMessage { get; set; }
        public PropertiesDictionary Properties { get; set; }
        public object MessageObject { get; set; }
        public string LoggerName { get; set; }
        public LocationInfo LocationInformation { get; set; }
        public string Identity { get; set; }
        public Exception ExceptionObject { get; set; }
        public string ExceptionString { get; set; }
        public LoggingEventData LoggingEventData { get; set; }
    }
}