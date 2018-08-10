using System;

namespace IdentityProvider.Infrastructure.Logging.Serilog.AuditLog
{
    public interface IAuditLogService : IDisposable
    {
        void LogInfo(object logSource, string message, Exception exception = null);
        void LogWarning(object logSource, string message, Exception exception = null);
        void LogError(object logSource, string message, Exception exception = null);
        void LogFatal(object logSource, string message, Exception exception = null);
        void LogInfoWithContext(object logSource, string message, Exception exception = null);
        void LogWarningWithContext(object logSource, string message, Exception exception = null);
        void LogErrorWithContext(object logSource, string message, Exception exception = null);
        void LogFatalWithContext(object logSource, string message, Exception exception = null);
        void LogVerbose(object logSource, string message, Exception exception = null);
        void LogVerboseWithContext(object logSource, string message, Exception exception = null);
        void InitializeLogger();
        void EnrichWithContextualData();
    }
}