using System;
using log4net.Appender;
using log4net.Core;

namespace IdentityProvider.Infrastructure.Logging.Log4Net
{
    public interface ILog4NetLoggingService : IDisposable
    {
        MemoryAppender Appender { get; set; }
        LoggingEvent LogEvent { get; set; }
        void LogInfo(object logSource, string message, Exception exception = null, bool viaWcf = false);
        void LogWarning(object logSource, string message, Exception exception = null, bool viaWcf = false);
        void LogError(object logSource, string message, Exception exception = null, bool viaWcf = false);
        void LogFatal(object logSource, string message, Exception exception = null, bool viaWcf = false);

        void LogDbTrace(string database, string procedureOrTypeOfExecuted, TimeSpan stopwatchElapsed,
            string commandText,
            string command, bool viaWcf = false);

        void LogDbError(string database, Exception exception, string commandText, string command, bool viaWcf = false);
    }
}