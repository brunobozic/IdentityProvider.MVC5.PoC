
using IdentityProvider.Services.Logging.WCF;
using Logging.WCF.Infrastructure;
using Logging.WCF.Infrastructure.Contracts;
using StructureMap;
using System;

namespace HAC.Helpdesk.Services.Logging.WCF
{
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        private readonly string _applicationId;
        private readonly bool _demoEnvironment;

        private readonly string _instanceId;
        private readonly string _log4NetSettingsFile;

        private readonly bool _loginByUserAndPassEnabled;
        private readonly bool _productionEnvironment;

        private readonly string _serilogGraylogAdress;

        private readonly string _serilogGraylogPort;

        private readonly string _serilogRollingLogLogFileName;

        private readonly string _serilogRollingLogLogPath;

        private readonly string _serilogRollingLogLogTemplate;

        private readonly string _serilogSqlLiteAuditLogFileName;

        private readonly string _serilogSqlLiteAuditLogPath;

        private readonly string _serilogSqlLiteErrorLogFileName;

        private readonly string _serilogSqlLiteErrorLogPath;
        private readonly bool _serilogSqlLitePerformanceLogRequestResponseEnabled;


        private readonly bool _serilogUsesGraylog;

        private readonly bool _shouldSendEmailWhenCertificateExpiryDateValidationFails;
        private readonly bool _shouldVerifyCertificateExpirationDate;
        private readonly bool _testEnvironment;


        private readonly string _wcfLoggingServiceURL;

        [DefaultConstructor]
        public ApplicationConfiguration(IConfigurationProvider configurationProvider,
            bool loginByUserAndPassEnabled = true)
        {
            var configurationProvider1 = configurationProvider;
            _loginByUserAndPassEnabled = loginByUserAndPassEnabled;

            _wcfLoggingServiceURL =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound("LoggingServiceURL",
                    "http://localhost:61234/LogWCF.svc");
            _log4NetSettingsFile =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound("Log4NetSettingsFile",
                    "log4Net.config.xml");
            // The basics...
            _applicationId =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound("ApplicationId",
                    "App1");
            _instanceId =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound("InstanceId", "1");


            // Test Environment...
            _testEnvironment =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound("TestEnvironment",
                    false);
            // Demo Environment...
            _demoEnvironment =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound("DemoEnvironment",
                    true);
            // Production Environment...
            _productionEnvironment =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound(
                    "ProductionEnvironment", false);

            // Logging stuff...
            _serilogUsesGraylog =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound("SerilogUsesGraylog",
                    true);
            _serilogGraylogAdress =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound("SerilogGraylogAdress",
                    "10.88.2.0");
            _serilogGraylogPort =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound("SerilogGraylogPort",
                    "9001");
            _serilogRollingLogLogPath =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound(
                    "SerilogRollingLogLogPath", "C:\\Logs\\");
            _serilogRollingLogLogFileName =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound(
                    "SerilogRollingLogLogFileName", "NIAS-log-{Date}.txt");
            _serilogRollingLogLogTemplate =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound(
                    "SerilogRollingLogLogTemplate",
                    "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}");
            _serilogSqlLiteErrorLogPath =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound(
                    "SerilogSqlLiteErrorLogPath", "C:\\Logs\\");
            _serilogSqlLiteErrorLogFileName =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound(
                    "SerilogSqlLiteErrorLogFileName", "NIAS-Error-LogToWCF.db");
            _serilogSqlLiteAuditLogPath =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound(
                    "SerilogSqlLiteAuditLogPath", "C:\\Logs\\");
            _serilogSqlLiteAuditLogFileName =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound(
                    "SerilogSqlLiteAuditLogFileName", "NIAS-Audit-LogToWCF.db");
            SerilogSqlLitePerformanceLogFileName =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound(
                    "SerilogSqlLitePerformanceLogFileName", "NIAS-Performance-LogToWCF.db");
            SerilogSqlLitePerformanceLogPath =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound(
                    "SerilogSqlLitePerformanceLogPath", "C:\\Logs\\");
            _serilogSqlLitePerformanceLogRequestResponseEnabled =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound(
                    "SerilogSqlLitePerformanceLogRequestResponseEnabled", false);

            // App certificate expiry date validation and email notification sending...
            _shouldVerifyCertificateExpirationDate =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound(
                    "ShouldVerifyCertificateExpirationDate", false);
            _shouldSendEmailWhenCertificateExpiryDateValidationFails =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound(
                    "ShouldSendEmailWhenCertificateExpiryDateValidationFails", false);

            // Application Performance logging and monitoring
            SerilogSqlLitePerformanceLogPath =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound(
                    "SerilogSqlLitePerformanceLogPath", "C:\\Logs\\");
            SerilogSqlLitePerformanceLogFileName =
                configurationProvider1.GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound(
                    "serilogSqlLitePerformanceLogFileName", "NIAS-Performance-LogToWCF.db");
        }

        public string GetAppCertThumbprintByType(CertificateTypeEnum certificateType)
        {
            throw new NotImplementedException();
        }

        public string GetApplicationId()
        {
            return _applicationId;
        }

        public AppEnvironmentEnum GetCurrentEnvironment()
        {
            var ne = AppEnvironmentEnum.Null;

            if (GetDemoEnvironment())
                ne = AppEnvironmentEnum.Demo;
            else if (GetTestEnvironment())
                ne = AppEnvironmentEnum.Test;
            else if (GetProductionEnvironment())
                ne = AppEnvironmentEnum.Production;

            return ne;
        }

        public bool GetDemoEnvironment()
        {
            return _demoEnvironment;
        }

        public string GetDontLogAnythingViaWCF()
        {
            return "true";
        }

        public string GetInstanceId()
        {
            return _instanceId;
        }

        public string GetLocationOfLog4NetConfigFile()
        {
            return _log4NetSettingsFile;
        }

        public string GetLogEverythingToFile()
        {
            return "false";
        }

        public string GetLogEverythingViaWCF()
        {
            return "false";
        }

        public bool GetLoginByUserAndPassEnabled()
        {
            return _loginByUserAndPassEnabled;
        }

        public string GetMailPassword()
        {
            throw new NotImplementedException();
        }

        public bool GetProductionEnvironment()
        {
            return _productionEnvironment;
        }

        public string GetSendGridApiKey()
        {
            throw new NotImplementedException();
        }

        public string GetSerilogGraylogAdress()
        {
            return _serilogGraylogAdress;
        }

        public string GetSerilogGraylogPort()
        {
            return _serilogGraylogPort;
        }

        public string GetSerilogRollingLogLogFileName()
        {
            return _serilogRollingLogLogFileName;
        }

        public string GetSerilogRollingLogLogPath()
        {
            return _serilogRollingLogLogPath;
        }

        public string GetSerilogRollingLogLogTemplate()
        {
            return _serilogRollingLogLogTemplate;
        }

        public string GetSerilogSqlLiteAuditLogFileName()
        {
            return _serilogSqlLiteAuditLogFileName;
        }

        public string GetSerilogSqlLiteAuditLogPath()
        {
            return _serilogSqlLiteAuditLogPath;
        }

        public string GetSerilogSqlLiteErrorLogFileName()
        {
            return _serilogSqlLiteErrorLogFileName;
        }

        public string GetSerilogSqlLiteErrorLogPath()
        {
            return _serilogSqlLiteErrorLogPath;
        }

        public bool GetSerilogUsesGraylog()
        {
            return _serilogUsesGraylog;
        }

        public bool GetTestEnvironment()
        {
            return _testEnvironment;
        }

        public string GetWCFLoggingServiceURL()
        {
            return _wcfLoggingServiceURL;
        }

        public bool SerilogSqlLitePerformanceLogRequestResponseEnabled()
        {
            return _serilogSqlLitePerformanceLogRequestResponseEnabled;
        }


        public bool ShouldSendEmailWhenCertificateExpiryDateValidationFails()
        {
            return _shouldSendEmailWhenCertificateExpiryDateValidationFails;
        }

        public bool ShouldVerifyCertificateExpirationDate()
        {
            return _shouldVerifyCertificateExpirationDate;
        }

        public string SerilogSqlLitePerformanceLogFileName { get; }


        public string SerilogSqlLitePerformanceLogPath { get; }
    }
}