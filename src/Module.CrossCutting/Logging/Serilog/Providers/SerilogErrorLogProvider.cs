using Module.CrossCutting.ApplicationContext;
using Serilog;

namespace Module.CrossCutting.Logging.Serilog.Providers
{
    public class SerilogErrorLogProvider : IErrorLogService
    {
        private readonly IAddLoggingContextProvider _loggingContext;
        private readonly ISerilogLoggingFactory _loggingFactory;
        private ILogger _loggingService;

        public SerilogErrorLogProvider(
            ISerilogLoggingFactory loggingFactory
            , IAddLoggingContextProvider addLoggingContext
        )
        {
            _loggingFactory = loggingFactory ?? throw new ArgumentNullException(nameof(loggingFactory));
            _loggingContext = addLoggingContext ?? throw new ArgumentNullException(nameof(addLoggingContext));

            SetupErrorLogger();
        }

        public void Dispose()
        {
        }

        public void EnrichWithContextualData()
        {
            // _loggingFactory.GetBaseLogger();
        }

        public void SetupErrorLogger()
        {
            _loggingService = _loggingFactory.GetLogger(SerilogLogTypesEnum.ErrorRollingLog);
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
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        ///     Returns and instance of serilog that sinks logs to Graylog
        /// </summary>
        /// <returns></returns>
        public void SetupGraylogLogger()
        {
            _loggingService = _loggingFactory.GetLogger(SerilogLogTypesEnum.Graylog);
        }


        private object GetUserName()
        {
            return _loggingContext.GetUserName() ?? "Anonymous";
        }
    }
}