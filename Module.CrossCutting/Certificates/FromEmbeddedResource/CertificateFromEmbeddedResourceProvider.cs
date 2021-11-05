using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using IdentityProvider.Infrastructure.Certificates.ExpiryValidation;
using IdentityProvider.Infrastructure.Enums;
using IdentityProvider.Infrastructure.Logging.Serilog.Providers;

namespace IdentityProvider.Infrastructure.Certificates.FromEmbeddedResource
{
    public class CertificateFromEmbeddedResourceProvider : ICertificateFromEmbededResourceProvider
    {
        #region Private properties

        private readonly IErrorLogService _errorLog;
        private readonly ICertificateExpirationValidator _certificateExpirationValidator;
        private X509Certificate2 certificate;

        #endregion Private properties

        #region Public Properties

        #endregion Public Properties

        #region Ctor

        public CertificateFromEmbeddedResourceProvider(
            IErrorLogService errorLog,
            ICertificateExpirationValidator certificateExpirationValidator
          )
        {
            _errorLog = errorLog;
            _certificateExpirationValidator = certificateExpirationValidator;
        

            if (_errorLog == null) throw new ArgumentNullException(nameof(_errorLog));
            if (_certificateExpirationValidator == null)
                throw new ArgumentNullException(nameof(_certificateExpirationValidator));
      
        }

        public X509Certificate2 GetValidCertificateFromEmbeddedResource()
        {
            const string testPwd = "";
            const string demoPwd = "";
            const string productionPwd = "";

            //if (_applicationConfiguration.GetCurrentEnvironment() == AppEnvironmentEnum.Test)
            //{
            //    var testApplicationCertificate = new X509Certificate2(string.Empty, testPwd);

            //    LogExtraInformation(
            //        string.Empty,
            //        testApplicationCertificate,
            //        testPwd,
            //        CertificateTypeEnum.TestApplication
            //    );

            //    certificate = testApplicationCertificate;
            //}
            //else if (_applicationConfiguration.GetCurrentEnvironment() == AppEnvironmentEnum.Demo)
            //{
            //    var demoApplicationCertificate = new X509Certificate2(string.Empty, demoPwd);

            //    LogExtraInformation(
            //        string.Empty,
            //        demoApplicationCertificate,
            //        demoPwd,
            //        CertificateTypeEnum.DemoApplication
            //    );

            //    certificate = demoApplicationCertificate;
            //}
            //else if (_applicationConfiguration.GetCurrentEnvironment() == AppEnvironmentEnum.Production)
            //{
            //    _errorLog.LogInfo(
            //        this,
            //        string.Format("APP_CERT_PRE_FETCH_CHECK", certificate == null)
            //    );

            //    X509Certificate2 productionApplicationCertificate = null;

            //    try
            //    {
            //        _errorLog.LogInfo(this,
            //            string.Format("APP_CERT_LENGTH_OF_EMBEDDED_RESOURCE", string.Empty?.Length)
            //        );

            //        productionApplicationCertificate = new X509Certificate2(string.Empty, productionPwd);
            //    }
            //    catch (Exception certificateException)
            //    {
            //        _errorLog.LogFatal(this, certificateException.Message, certificateException);
            //    }

            //    _errorLog.LogInfo(
            //        this,
            //        string.Format("APP_CERT_GOT_CERT_FROM_EMBEDDED_RESOURCE",
            //            productionApplicationCertificate?.SubjectName.Name)
            //    );

            //    certificate = productionApplicationCertificate;
            //}

            // deal breaker....
            if (certificate == null) throw new NoNullAllowedException(nameof(certificate));


            //if (_applicationConfiguration.ShouldVerifyCertificateExpirationDate())
            //{
            //    var validationResult = VerifyCertificateExpirationDate(certificate, CertificateTypeEnum.Application);
            //}

            return certificate;
        }

        #endregion Ctor

        #region Public methods

        #endregion Public methods

        #region Private methods

        private void LogExtraInformation(
            string nameOfResourceFile,
            X509Certificate2 certificate,
            string certificatePassword,
            CertificateTypeEnum certificateType
        )
        {
            switch (certificateType)
            {
                case CertificateTypeEnum.Application:

                    break;
                case CertificateTypeEnum.Validation:

                    break;
                case CertificateTypeEnum.TestApplication:

                    break;
                case CertificateTypeEnum.TestValidation:

                    break;
            }

            _errorLog.LogInfo(
                this,
                string.Format(
                    "APP_CERT_FOUND_MATCHING_CERT_BY_EMBEDDED_RESOURCE_NAME",
                    certificateType,
                    nameOfResourceFile,
                    certificate?.SubjectName,
                    certificate?.GetExpirationDateString()
                )
            );
        }

        private CertificateValidationReponse VerifyCertificateExpirationDate(X509Certificate2 certificate,
            CertificateTypeEnum application)
        {
            return _certificateExpirationValidator.Validate(certificate, application);
        }

        #endregion Private methods
    }
}