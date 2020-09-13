using log4net.Appender;
using log4net.Core;
using System;

namespace Logging.WCF.Models.Log4Net
{
    public interface ILog4NetLoggingService : IDisposable
    {
        void LogError(object logSource, string message, Exception exception = null, bool viaWcf = false);
        void LogFatal(object logSource, string message, Exception exception = null, bool viaWcf = false);
        void LogInfo(object logSource, string message, Exception exception = null, bool viaWcf = false);
        void LogWarning(object logSource, string message, Exception exception = null, bool viaWcf = false);

        MemoryAppender Appender { get; set; }
        LoggingEvent LogEvent { get; set; }

        //void LogDbTrace(string database, string procedureOrTypeOfExecuted, TimeSpan stopwatchElapsed,
        //    string commandText,
        //    string command, bool viaWcf = false);

        //void LogDbError(string database, Exception exception, string commandText, string command, bool viaWcf = false);
    }
}