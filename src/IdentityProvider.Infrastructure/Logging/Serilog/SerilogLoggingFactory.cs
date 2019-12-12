using System;
using IdentityProvider.Infrastructure.Caching;
using IdentityProvider.Infrastructure.ConfigurationProvider;
using IdentityProvider.Infrastructure.Logging.Serilog.AuditLog;
using Serilog;



namespace IdentityProvider.Infrastructure.Logging.Serilog
{
    public class LoggingFactory : ISerilogLoggingFactory
    {
        private const string SqliteFilePathNotProvidedViaConfig =
            "Please provide the file path for Sqlite database file";

        private const string SqliteFilDbNameNotProvidedViaConfig =
            "Please provide the file name for Sqlite database file";

        private const string Err_Graylog_Settings_Invalid =
            "Unable to begin logging to Graylog because either the Graylog Ip or Port were not defined.";

        private const string RollingFileTemplateDefault =
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}";

        private readonly IConfigurationProvider _configurationRepository;
        private readonly IMemoryCacheProvider _memoryCacheProvider;
        private ILogger _internalSerilogLogger;
        private LoggerConfiguration _logConfiguration;
        private IAuditLogService loggingService;

        public LoggingFactory(IMemoryCacheProvider memoryCacheProvider, IConfigurationProvider configurationRepository)
        {
            _memoryCacheProvider = memoryCacheProvider ?? throw new ArgumentNullException(nameof(memoryCacheProvider));
            _configurationRepository = configurationRepository ??
                                       throw new ArgumentNullException(nameof(configurationRepository));
        }

        /// <summary>
        ///     Returns a preconfigured instance of serilog logger.
        ///     Audit log messages will be inserted into a local sqlite database.
        ///     Error messages on the other hand will be added on top of a rolling file log.
        ///     Graylog messages will be sent to a Graylog server (if one exists).
        /// </summary>
        /// <param name="niasMessageAudit"></param>
        /// <returns></returns>
        public ILogger GetLogger(SerilogLogTypesEnum niasMessageAudit)
        {
            var rollingLogLogPath =
                _configurationRepository.GetConfigurationValueAndNotifyIfPropertyNotFound<string>(
                    "SerilogRollingLogLogPath");
            var rollingLogLogFileName =
                _configurationRepository.GetConfigurationValueAndNotifyIfPropertyNotFound<string>(
                    "SerilogRollingLogLogFileName");
            var rollingLogLogTemplate =
                _configurationRepository.GetConfigurationValueAndNotifyIfPropertyNotFound<string>(
                    "SerilogRollingLogLogTemplate");
            var SqliteErrorLogPath =
                _configurationRepository.GetConfigurationValueAndNotifyIfPropertyNotFound<string>(
                    "SerilogSqlLiteErrorLogPath");
            var SqliteErrorLogFileName =
                _configurationRepository.GetConfigurationValueAndNotifyIfPropertyNotFound<string>(
                    "SerilogSqlLiteErrorLogFileName");
            var SqliteAuditLogPath =
                _configurationRepository.GetConfigurationValueAndNotifyIfPropertyNotFound<string>(
                    "SerilogSqlLiteAuditLogPath");
            var SqliteAuditLogFileName =
                _configurationRepository.GetConfigurationValueAndNotifyIfPropertyNotFound<string>(
                    "SerilogSqlLiteAuditLogFileName");
            var SqlitePerformanceLogPath =
                _configurationRepository.GetConfigurationValueAndNotifyIfPropertyNotFound<string>(
                    "SerilogSqlLitePerformanceLogPath");
            var SqlitePerformanceLogFileName =
                _configurationRepository.GetConfigurationValueAndNotifyIfPropertyNotFound<string>(
                    "SerilogSqlLitePerformanceLogFileName");
            var shouldUseGraylog =
                _configurationRepository.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound("SerilogUsesGraylog",
                    false);
            var graylogIP =
                _configurationRepository.GetConfigurationValueAndNotifyIfPropertyNotFound<string>(
                    "SerilogGraylogAdress");
            var graylogPort =
                _configurationRepository.GetConfigurationValueAndNotifyIfPropertyNotFound<string>("SerilogGraylogPort");

            switch (niasMessageAudit)
            {
                case SerilogLogTypesEnum.PerformanceLog:
                    if (!string.IsNullOrEmpty(SqliteAuditLogPath) && !string.IsNullOrEmpty(SqliteAuditLogFileName))
                    {
                        _logConfiguration = new LoggerConfiguration().WriteTo.SQLitePerformanceAudit(
                            $"{SqlitePerformanceLogPath}{SqlitePerformanceLogFileName}");

                        if (_memoryCacheProvider.Get<ILogger>("PerformanceLog") == null)
                        {
                            _internalSerilogLogger = _logConfiguration.CreateLogger();
                            _memoryCacheProvider.Save(_internalSerilogLogger, "PerformanceLog");
                        }
                        else
                        {
                            return _memoryCacheProvider.Get<ILogger>("PerformanceLog");
                        }
                    }

                    break;

                //case SerilogLogTypesEnum.Graylog:
                //    if (shouldUseGraylog)
                //        if (!string.IsNullOrEmpty(graylogIP) && !string.IsNullOrEmpty(graylogPort))
                //        {
                //            _logConfiguration = new LoggerConfiguration().WriteTo.Graylog(new GraylogSinkOptions
                //            {
                //                HostnameOrAdress = graylogIP,
                //                Port = Convert.ToInt32(graylogPort)
                //            });

                //            if (_memoryCacheProvider.Get<ILogger>("GraylogLog") == null)
                //            {
                //                _internalSerilogLogger = _logConfiguration.CreateLogger();
                //                _memoryCacheProvider.Save(_internalSerilogLogger, "GraylogLog");
                //            }
                //            else
                //            {
                //                return _memoryCacheProvider.Get<ILogger>("GraylogLog");
                //            }
                //        }
                //        else
                //        {
                //            _internalSerilogLogger.Error(Err_Graylog_Settings_Invalid);
                //        }

                //    break;

                case SerilogLogTypesEnum.ErrorRollingLog:
                    if (!string.IsNullOrEmpty(rollingLogLogPath) && !string.IsNullOrEmpty(rollingLogLogFileName))
                    {
                        _logConfiguration = new LoggerConfiguration().WriteTo.RollingFile(
                            rollingLogLogPath + rollingLogLogFileName, outputTemplate: rollingLogLogTemplate);

                        _internalSerilogLogger = _logConfiguration.CreateLogger();

                        if (_memoryCacheProvider.Get<ILogger>("ErrorLogInFile") == null)
                            _memoryCacheProvider.Save(_internalSerilogLogger, "ErrorLogInFile");
                        else return _memoryCacheProvider.Get<ILogger>("ErrorLogInFile");
                    }
                    break;
            }

            return _internalSerilogLogger;
        }
    }
}