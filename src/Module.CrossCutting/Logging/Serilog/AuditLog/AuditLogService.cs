using Module.CrossCutting.ApplicationContext;
using Module.CrossCutting.Enums;
using Serilog;

namespace Module.CrossCutting.Logging.Serilog.AuditLog
{
    public class SerilogAuditLogProvider : IAuditLogService
    {
        private const string pattern =
            "{MsgType}{CurrentEnvironment}{ApplicationId}{StatusMessage}{StatusCode}{Destination}{Reason}{NiasError}";

        private readonly IAddLoggingContextProvider _loggingContext;
        private readonly ISerilogLoggingFactory _loggingFactory;
        private string _applicationId;
        private AppEnvironmentEnum _environment;
        private ILogger _loggingService;

        public SerilogAuditLogProvider(
            ISerilogLoggingFactory loggingFactory,
            IAddLoggingContextProvider addLoggingContext,
            AppEnvironmentEnum environment)
        {
            _loggingFactory = loggingFactory ?? throw new ArgumentNullException(nameof(loggingFactory));
            _loggingContext = addLoggingContext ?? throw new ArgumentNullException(nameof(addLoggingContext));
            _environment = environment;

            if (_loggingService == null)
                _loggingService = _loggingFactory.GetLogger(SerilogLogTypesEnum.NiasMessageAudit);
        }

        public void Dispose()
        {
        }

        public void EnrichWithContextualData()
        {
            // _loggingFactory.GetBaseLogger();
        }

        public void InitializeLogger()
        {
            _loggingService = _loggingFactory.GetLogger(SerilogLogTypesEnum.NiasMessageAudit);
        }

        public void LogError(object logSource, string message, Exception exception = null)
        {
            _loggingService.Error(exception, message);
        }

        public void LogErrorWithContext(object logSource, string message, Exception exception = null)
        {
            _loggingService.Error(exception, message);
        }

        public void LogFatal(object logSource, string message, Exception exception = null)
        {
            _loggingService.Fatal(exception, message);
        }

        public void LogFatalWithContext(object logSource, string message, Exception exception = null)
        {
            _loggingService.Fatal(exception, message);
        }

        public void LogInfo(object logSource, string message, Exception exception = null)
        {
            _loggingService.Information(exception, message);
        }

        public void LogWarning(object logSource, string message, Exception exception = null)
        {
            _loggingService.Warning(exception, message);
        }

        public void LogWarningWithContext(object logSource, string message, Exception exception = null)
        {
            _loggingService.Warning(exception, message);
        }

        public void LogVerbose(object logSource, string message, Exception exception = null)
        {
            _loggingService.Verbose(exception, message);
        }

        public void LogVerboseWithContext(object logSource, string message, Exception exception = null)
        {
            _loggingService.Verbose(exception, message);
        }

        public void LogInfoWithContext(object logSource, string message, Exception exception = null)
        {
            _loggingService.Information(exception, message);
        }

        private void AddProperties(object logSource, Exception exception)
        {
            //loggingEvent.Properties["UserName"] = GetUserName();

            try
            {
                //    var contextProperties = _loggingContext.GetContextProperties();

                //    if (contextProperties != null)
                //    {
                //        try
                //        {
                //            loggingEvent.Properties["UserAgent"] = contextProperties.UserAgent;
                //            loggingEvent.Properties["RemoteHost"] = contextProperties.RemoteHost;
                //            loggingEvent.Properties["Path"] = contextProperties.Path;
                //            loggingEvent.Properties["Query"] = contextProperties.Query;
                //            loggingEvent.Properties["RefererUrl"] = contextProperties.Referrer;
                //            loggingEvent.Properties["RequestId"] = contextProperties.RequestId;
                //            loggingEvent.Properties["SessionId"] = contextProperties.SessionId;
                //        }
                //        catch (Exception exc)
                //        {

                //        }
                //    }

                //    loggingEvent.Properties["ExceptionType"] = exception == null ? "" : exception.GetType().ToString();
                //    loggingEvent.Properties["ExceptionMessage"] = exception == null ? "" : exception.Message;
                //    loggingEvent.Properties["ExceptionStackTrace"] = exception == null ? "" : exception.StackTrace;

                //    if (exception != null)
                //    {
                //        if (exception.InnerException != null)
                //        {
                //            loggingEvent.Properties["InnerException.Message"] = exception.InnerException.Message;
                //            loggingEvent.Properties["InnerException.Source"] = exception.InnerException.Source ?? "";
                //            loggingEvent.Properties["InnerException.StackTrace"] = exception.InnerException.StackTrace ?? "";
                //            loggingEvent.Properties["InnerException.TargetSite"] = exception.InnerException.TargetSite == null
                //                ? ""
                //                : exception.InnerException.TargetSite.ToString();
                //        }
                //    }

                //    loggingEvent.Properties["AssemblyQualifiedName"] = exception == null
                //        ? ""
                //        : exception.GetType().AssemblyQualifiedName;
                //    loggingEvent.Properties["Namespace"] = exception == null ? "" : exception.GetType().Namespace;

                //    if (logSource != null)
                //    {
                //        loggingEvent.Properties["LogSource"] = logSource.GetType().ToString();
                //    }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private object GetUserName()
        {
            return _loggingContext.GetUserName() ?? "Anonymous";
        }


        #region Log Messages

        #endregion Log Messages
    }
}