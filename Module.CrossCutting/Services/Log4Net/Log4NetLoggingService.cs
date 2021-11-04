using System;
using System.IO;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Logging.WCF.Infrastructure.Contracts;
using Logging.WCF.Models.Log4Net;
using Logging.WCF.Services.SampleManagerCode;
using IAddLoggingContextProvider = IdentityProvider.Infrastructure.ApplicationContext.IAddLoggingContextProvider;
using IApplicationConfiguration = IdentityProvider.Infrastructure.ApplicationConfiguration.IApplicationConfiguration;

namespace IdentityProvider.Services.Log4Net
{
    public class Log4NetLoggingService : ILog4NetLoggingService
    {
        private readonly IApplicationConfiguration _configurationRepository;
        private readonly IAddLoggingContextProvider _contextService;
        private readonly string _dontLogAnythingViaWcf;
        private readonly bool _isMock;
        private readonly string _log4NetConfigFileName;
        private readonly string _logEverythingToFile;
        private readonly string _logEverythingViaWcf;
        private ILog _logManager;
        private readonly IWcfLoggingManager _wcfLoggingManager;

        public Log4NetLoggingService(
            IApplicationConfiguration configurationRepository
            , IAddLoggingContextProvider contextService
            , bool isMock = false
        )
        {
            _isMock = isMock;
            _configurationRepository = configurationRepository ??
                                       throw new ArgumentNullException(nameof(configurationRepository));
            _contextService = contextService ?? throw new ArgumentNullException(nameof(contextService));
            _wcfLoggingManager = new WCFLoggingManager(_configurationRepository.GetWCFLoggingServiceURL());

            try
            {
                _log4NetConfigFileName = _configurationRepository.GetLocationOfLog4NetConfigFile();
            }
            catch (Exception)
            {
                throw new ApplicationException("Log4NetSettingsFile key missing from web/app configuration.");
            }

            if (string.IsNullOrEmpty(_log4NetConfigFileName))
                throw new ApplicationException("Log4NetSettingsFile key missing from web/app configuration.");

            try
            {
                // Override setting, logs everything to database (via windows communication foundation)
                _logEverythingViaWcf = _configurationRepository.GetLogEverythingViaWCF();
            }
            catch (Exception)
            {
                // fallback, default is false
                _logEverythingViaWcf = "false";
            }

            try
            {
                // Override setting, disables all logging to database (via windows communication foundation)
                _dontLogAnythingViaWcf = _configurationRepository.GetDontLogAnythingViaWCF();
            }
            catch (Exception)
            {
                // fallback, default is true
                _dontLogAnythingViaWcf = "true";
            }


            try
            {
                // Override setting, disables all logging to database (via windows communication foundation)
                _logEverythingToFile = _configurationRepository.GetLogEverythingToFile();
            }
            catch (Exception)
            {
                // fallback, default is true
                _logEverythingToFile = "true";
            }

            SetupLogger();
        }

        public void Dispose()
        {
            // TODO:
        }

        public void LogError(object logSource, string message, Exception exception = null, bool viaWcf = false)
        {
            LogMessageWithProperties(logSource, message, Level.Error, exception, viaWcf);
        }

        public void LogFatal(object logSource, string message, Exception exception = null, bool viaWcf = false)
        {
            LogMessageWithProperties(logSource, message, Level.Fatal, exception, viaWcf);
        }

        public void LogInfo(object logSource, string message, Exception exception = null, bool viaWcf = false)
        {
            LogMessageWithProperties(logSource, message, Level.Info, exception, viaWcf);
        }

        public void LogWarning(object logSource, string message, Exception exception = null, bool viaWcf = false)
        {
            LogMessageWithProperties(logSource, message, Level.Warn, exception, viaWcf);
        }

        public MemoryAppender Appender { get; set; }

        public LoggingEvent LogEvent { get; set; }

        private void AddProperties(
            object logSource
            , Exception exception
            , LoggingEvent loggingEvent
        )
        {
            loggingEvent.Properties["UserName"] = GetUserName();

            try
            {
                var contextProperties = _contextService.GetContextProperties();

                if (contextProperties != null)
                    try
                    {
                        loggingEvent.Properties["UserAgent"] = contextProperties.UserAgent;
                        loggingEvent.Properties["RemoteHost"] = contextProperties.RemoteHost;
                        loggingEvent.Properties["Path"] = contextProperties.Path;
                        loggingEvent.Properties["Query"] = contextProperties.Query;
                        loggingEvent.Properties["RefererUrl"] = contextProperties.Referrer;
                        loggingEvent.Properties["RequestId"] = contextProperties.RequestId;
                        loggingEvent.Properties["SessionId"] = contextProperties.SessionId;
                    }
                    catch (Exception exc)
                    {
                        var type = typeof(Log4NetLoggingService);
                        _logManager = LogManager.GetLogger(type);
                        _logManager.Logger.Log(type, Level.Fatal,
                            "Exception when extracting properties: " + exc.Message,
                            exc);
                    }

                loggingEvent.Properties["ExceptionType"] =
                    exception == null ? string.Empty : exception.GetType().ToString();
                loggingEvent.Properties["ExceptionMessage"] = exception == null ? string.Empty : exception.Message;
                loggingEvent.Properties["ExceptionStackTrace"] =
                    exception == null ? string.Empty : exception.StackTrace;

                if (exception?.InnerException != null)
                {
                    loggingEvent.Properties["InnerException.Message"] = exception.InnerException.Message;
                    loggingEvent.Properties["InnerException.Source"] = exception.InnerException.Source ?? string.Empty;
                    loggingEvent.Properties["InnerException.StackTrace"] =
                        exception.InnerException.StackTrace ?? string.Empty;
                    loggingEvent.Properties["InnerException.TargetSite"] = exception.InnerException.TargetSite ==
                                                                           null
                        ? string.Empty
                        : exception.InnerException.TargetSite.ToString();
                }

                loggingEvent.Properties["AssemblyQualifiedName"] = exception == null
                    ? string.Empty
                    : exception.GetType().AssemblyQualifiedName;
                loggingEvent.Properties["Namespace"] = exception == null ? string.Empty : exception.GetType().Namespace;

                if (logSource != null)
                    loggingEvent.Properties["LogSource"] = logSource.GetType().ToString();
            }
            catch (Exception ex)
            {
                var type = typeof(Log4NetLoggingService);
                _logManager = LogManager.GetLogger(type);
                _logManager.Logger.Log(type, Level.Fatal, "Exception when extracting properties: " + ex.Message, ex);
            }
        }

        private ILog GetMyLogger(
            object logSource
            , Level level
            , bool isMock = false
        )
        {
            ILog logger = null;

            // In production environment we always log to file...
            if (!isMock)
            {
                logger = LogManager.GetLogger(logSource.GetType());
            }
            else
            {
                LogManager.ResetConfiguration();

                // On the other hand, while unit testing we opt to use the memory appender...
                Appender = new MemoryAppender
                {
                    Name = "Unit Testing Appender",
                    Layout = new PatternLayout("%message"),
                    Threshold = level
                };

                Appender.ActivateOptions();
                var root = ((Hierarchy) LogManager.GetRepository()).Root;
                root.RemoveAllAppenders();
                root.AddAppender(Appender);
                root.Repository.Configured = true;


                logger = LogManager.GetLogger(logSource.GetType());
            }

            return logger;
        }

        private string GetUserName()
        {
            return _isMock ? _contextService.FakeUserNameForTestingPurposes : _contextService.GetUserName();
        }

        private void LogMessageWithProperties(
            object logSource
            , string message
            , Level level
            , Exception exception
            , bool logViaWcf = false
        )
        {
            _logManager = GetMyLogger(logSource, level, _isMock);

            LogEvent = new LoggingEvent(logSource.GetType(), _logManager.Logger.Repository, _logManager.Logger.Name,
                level,
                message,
                null);

            AddProperties(logSource, exception, LogEvent);

            try
            {
                // We want all exception/trace/audit/debug messages to be logged to file if the calling method was not explicitly called with "logViaWcf" parameter set to true
                // we also want all exception/trace/audit/debug messages to be logged to file if the "_logEverythingToFile" overrride was set to true (in application config)
                if (Convert.ToBoolean(_logEverythingToFile) || !logViaWcf)
                    _logManager.Logger.Log(LogEvent);
            }
            catch (AggregateException ae)
            {
                ae.Handle(x => true);
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                // We want all exception/trace/audit/debug messages to be logged to database via WCF if the calling method was explicitly called with "logViaWcf" parameter set to true
                // We also log to database via WCF if we have override property "_logEverythingViaWcf" set to true (in application config)
                // We also have one additional override property "_dontLogAnythingViaWcf", if this is set to true (in application config) we dont log anything to database (via WCF)
                if (logViaWcf || Convert.ToBoolean(_logEverythingViaWcf) && !Convert.ToBoolean(_dontLogAnythingViaWcf))
                    _wcfLoggingManager.LogToWCF(LogEvent);
            }
            catch (AggregateException ae)
            {
                ae.Handle(x => true);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void SetupLogger()
        {
            FileInfo log4NetSettingsFileInfo;

            var contextualPath = _contextService.GetContextualFullFilePath(_log4NetConfigFileName);

            if (!string.IsNullOrEmpty(contextualPath))
                log4NetSettingsFileInfo = new FileInfo(contextualPath);
            else
                throw new ApplicationException(string.Concat("Log4net settings file: [ ", _log4NetConfigFileName,
                    " ] not found as contextual path is null."));

            if (!log4NetSettingsFileInfo.Exists)
            {
                if (!string.IsNullOrEmpty(contextualPath))
                    throw new ApplicationException(string.Concat("Log4net settings file: [ ", _log4NetConfigFileName,
                        " ] not found at contextual path: ", contextualPath));
                throw new ApplicationException(string.Concat("Log4net settings file: [ ", _log4NetConfigFileName,
                    " ] not found."));
            }

            XmlConfigurator
                .ConfigureAndWatch(log4NetSettingsFileInfo);
        }

        //#region Database errors and trace data (logging)

        //// _logger.LogDbTrace("SQL Database", "SchoolInterceptor.ScalarExecuted", _stopwatch.Elapsed, "Command:",command.CommandText);
        //public void LogDbTrace(
        //    string database
        //    , string procedureOrTypeOfExecuted
        //    , TimeSpan stopwatchElapsed
        //    , string commandText
        //    , string command
        //    , bool viaWcf = false
        //)
        //{
        //    try
        //    {
        //        LogDbTraceWithProperties(database, procedureOrTypeOfExecuted, stopwatchElapsed, commandText, command,
        //            viaWcf);
        //    }
        //    catch (Exception)
        //    {
        //        // ignored
        //    }
        //}

        //public void LogDbError(string database, Exception exception, string commandText, string command,
        //    bool viaWcf = false)
        //{
        //    try
        //    {
        //        LogDbErrorWithProperties(database, exception, commandText, command, viaWcf);
        //    }
        //    catch (Exception)
        //    {
        //        // ignored
        //    }
        //}

        //private void LogDbTraceWithProperties(
        //    string database
        //    , string procedureOrTypeOfExecuted
        //    , TimeSpan stopwatchElapsed
        //    , string commandText
        //    , string command
        //    , bool logViaWcf
        //)
        //{
        //    var logger = LogManager.GetLogger(database);

        //    var loggingEvent = new LoggingEvent(GetType(), logger.Logger.Repository, logger.Logger.Name, Level.Trace,
        //        string.Format(commandText, command),
        //        null);

        //    AddPropertiesDbTrace(database, procedureOrTypeOfExecuted, stopwatchElapsed, commandText, command,
        //        loggingEvent);

        //    try
        //    {
        //        // We want all exception/trace/audit/debug messages to be logged to file if the calling method was not explicitly called with "logViaWcf" parameter set to true
        //        // we also want all exception/trace/audit/debug messages to be logged to file if the "_logEverythingToFile" overrride was set to true (in application config)
        //        if (Convert.ToBoolean(_logEverythingToFile) || !logViaWcf)
        //            logger.Logger.LogToWCF(loggingEvent);
        //    }
        //    catch (AggregateException ae)
        //    {
        //        ae.Handle(x => true);
        //    }
        //    catch (Exception)
        //    {
        //        // ignored
        //    }

        //    try
        //    {
        //        // We want all exception/trace/audit/debug messages to be logged to database via WCF if the calling method was explicitly called with "logViaWcf" parameter set to true
        //        // We also log to database via WCF if we have override property "_logEverythingViaWcf" set to true (in application config)
        //        // We also have one additional override property "_dontLogAnythingViaWcf", if this is set to true (in application config) we dont log anything to database (via WCF)
        //        if (logViaWcf || Convert.ToBoolean(_logEverythingViaWcf) && !Convert.ToBoolean(_dontLogAnythingViaWcf))
        //        {

        //            _wcfLoggingManager.LogToWcf(loggingEvent);
        //        }
        //    }
        //    catch (AggregateException ae)
        //    {
        //        ae.Handle(x => true);
        //    }
        //    catch (Exception)
        //    {
        //        // ignored
        //    }
        //}

        //private void AddPropertiesDbTrace(
        //    string database
        //    , string procedureOrTypeOfExecuted
        //    , TimeSpan stopwatchElapsed
        //    , string commandText
        //    , string command
        //    , LoggingEvent loggingEvent
        //)
        //{
        //    loggingEvent.Properties["UserName"] = GetUserName();

        //    try
        //    {
        //        var contextProperties = _contextService.GetContextProperties();

        //        if (contextProperties != null)
        //            try
        //            {
        //                loggingEvent.Properties["UserAgent"] = contextProperties.UserAgent;
        //                loggingEvent.Properties["RemoteHost"] = contextProperties.RemoteHost;
        //                loggingEvent.Properties["Path"] = contextProperties.Path;
        //                loggingEvent.Properties["Query"] = contextProperties.Query;
        //                loggingEvent.Properties["RefererUrl"] = contextProperties.Referrer;
        //                loggingEvent.Properties["RequestId"] = contextProperties.RequestId;
        //                loggingEvent.Properties["SessionId"] = contextProperties.SessionId;
        //            }
        //            catch (Exception exc)
        //            {
        //                var type = typeof(Log4NetLoggingService);
        //                var logger = LogManager.GetLogger(type);
        //                logger.Logger.LogToWCF(type, Level.Fatal, "Exception when extracting properties: " + exc.Message,
        //                    exc);
        //            }

        //        loggingEvent.Properties["Database"] = database;
        //        loggingEvent.Properties["ProcedureOrTypeOfExecuted"] = procedureOrTypeOfExecuted;
        //        loggingEvent.Properties["TotalTimeElapsed"] = stopwatchElapsed;
        //        loggingEvent.Properties["CommandText"] = commandText;
        //        loggingEvent.Properties["Command"] = command;
        //    }
        //    catch (Exception ex)
        //    {
        //        var type = typeof(Log4NetLoggingService);
        //        var logger = LogManager.GetLogger(type);
        //        logger.Logger.LogToWCF(type, Level.Fatal, "Exception when extracting properties: " + ex.Message, ex);
        //    }
        //}

        //private void LogDbErrorWithProperties(
        //    string database
        //    , Exception exception
        //    , string commandText
        //    , string command
        //    , bool logViaWcf
        //)
        //{
        //    var logger = LogManager.GetLogger(database);

        //    var loggingEvent = new LoggingEvent(GetType(), logger.Logger.Repository, logger.Logger.Name, Level.Trace,
        //        string.Format(commandText, command),
        //        null);

        //    AddPropertiesDbError(database, exception, commandText, command, loggingEvent);

        //    try
        //    {
        //        // We want all exception/trace/audit/debug messages to be logged to file if the calling method was not explicitly called with "logViaWcf" parameter set to true
        //        // we also want all exception/trace/audit/debug messages to be logged to file if the "_logEverythingToFile" overrride was set to true (in application config)
        //        if (Convert.ToBoolean(_logEverythingToFile) || !logViaWcf)
        //            logger.Logger.LogToWCF(loggingEvent);
        //    }
        //    catch (AggregateException ae)
        //    {
        //        ae.Handle(x => true);
        //    }
        //    catch (Exception)
        //    {
        //        // ignored
        //    }

        //    try
        //    {
        //        // We want all exception/trace/audit/debug messages to be logged to database via WCF if the calling method was explicitly called with "logViaWcf" parameter set to true
        //        // We also log to database via WCF if we have override property "_logEverythingViaWcf" set to true (in application config)
        //        // We also have one additional override property "_dontLogAnythingViaWcf", if this is set to true (in application config) we dont log anything to database (via WCF)
        //        if (logViaWcf || Convert.ToBoolean(_logEverythingViaWcf) && !Convert.ToBoolean(_dontLogAnythingViaWcf))
        //        {


        //            _wcfLoggingManager.LogToWcf(loggingEvent);
        //        }
        //    }
        //    catch (AggregateException ae)
        //    {
        //        ae.Handle(x => true);
        //    }
        //    catch (Exception)
        //    {
        //        // ignored
        //    }
        //}

        //private void AddPropertiesDbError(
        //    string database
        //    , Exception exception
        //    , string commandText
        //    , string command
        //    , LoggingEvent loggingEvent)
        //{
        //    loggingEvent.Properties["UserName"] = GetUserName();

        //    try
        //    {
        //        var contextProperties = _contextService.GetContextProperties();

        //        if (contextProperties != null)
        //            try
        //            {
        //                loggingEvent.Properties["UserAgent"] = contextProperties.UserAgent;
        //                loggingEvent.Properties["RemoteHost"] = contextProperties.RemoteHost;
        //                loggingEvent.Properties["Path"] = contextProperties.Path;
        //                loggingEvent.Properties["Query"] = contextProperties.Query;
        //                loggingEvent.Properties["RefererUrl"] = contextProperties.Referrer;
        //                loggingEvent.Properties["RequestId"] = contextProperties.RequestId;
        //                loggingEvent.Properties["SessionId"] = contextProperties.SessionId;
        //            }
        //            catch (Exception exc)
        //            {
        //                var type = typeof(Log4NetLoggingService);
        //                var logger = LogManager.GetLogger(type);
        //                logger.Logger.LogToWCF(type, Level.Fatal, "Exception when extracting properties: " + exc.Message,
        //                    exc);
        //            }

        //        if (exception != null)
        //            loggingEvent.Properties["DatabaseError"] = exception;

        //        loggingEvent.Properties["Database"] = database;
        //        loggingEvent.Properties["CommandText"] = commandText;
        //        loggingEvent.Properties["Command"] = command;
        //    }
        //    catch (Exception ex)
        //    {
        //        var type = typeof(Log4NetLoggingService);
        //        var logger = LogManager.GetLogger(type);
        //        logger.Logger.LogToWCF(type, Level.Fatal, "Exception when extracting properties: " + ex.Message, ex);
        //    }
        //}

        //#endregion
    }
}