using System;
using log4net.Appender;
using log4net.Core;

namespace IdentityProvider.Infrastructure.Logging.Log4Net
{
    public class NoLogService : ILog4NetLoggingService
    {
        public void Dispose()
        {
        }

        public MemoryAppender Appender { get; set; }
        public LoggingEvent LogEvent { get; set; }

        public void LogInfo(object logSource, string message, Exception exception = null, bool viaWcf = false)
        {
        }

        public void LogWarning(object logSource, string message, Exception exception = null, bool viaWcf = false)
        {
        }

        public void LogError(object logSource, string message, Exception exception = null, bool viaWcf = false)
        {
        }

        public void LogFatal(object logSource, string message, Exception exception = null, bool viaWcf = false)
        {
        }

        public void LogDbTrace(string database, string procedureOrTypeOfExecuted, TimeSpan stopwatchElapsed,
            string commandText,
            string command, bool viaWcf = false)
        {
        }

        public void LogDbError(string database, Exception exception, string commandText, string command,
            bool viaWcf = false)
        {
        }
    }
}