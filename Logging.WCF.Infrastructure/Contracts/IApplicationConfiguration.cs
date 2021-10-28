namespace Logging.WCF.Infrastructure.Contracts
{
    public interface IApplicationConfiguration
    {
        bool ShouldSendEmailWhenCertificateExpiryDateValidationFails();

        bool ShouldVerifyCertificateExpirationDate();

        /// <summary>
        ///     Returns true if web.config app key defines that NIAS should validate the SOAP messages incoming from NIAS.
        /// </summary>
        /// <returns></returns>
        bool GetLoginByUserAndPassEnabled();

        string GetApplicationId();
        string GetInstanceId();


        bool GetSerilogUsesGraylog();
        string GetSerilogGraylogAdress();
        string GetSerilogGraylogPort();
        string GetSerilogRollingLogLogPath();
        string GetSerilogRollingLogLogFileName();
        string GetSerilogRollingLogLogTemplate();
        string GetWCFLoggingServiceURL();
        string GetSerilogSqlLiteErrorLogPath();
        string GetSerilogSqlLiteErrorLogFileName();
        string GetSerilogSqlLiteAuditLogPath();
        string GetSerilogSqlLiteAuditLogFileName();


        bool SerilogSqlLitePerformanceLogRequestResponseEnabled();
        AppEnvironmentEnum GetCurrentEnvironment();
        string GetAppCertThumbprintByType(CertificateTypeEnum certificateType);
        string GetLocationOfLog4NetConfigFile();
        string GetLogEverythingViaWCF();
        string GetDontLogAnythingViaWCF();
        string GetLogEverythingToFile();
        string GetSendGridApiKey();
        string GetMailPassword();
    }
}