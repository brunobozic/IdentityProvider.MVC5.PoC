using System;
using System.Security.Cryptography.X509Certificates;
using IdentityProvider.Infrastructure.ApplicationConfiguration;
using IdentityProvider.Infrastructure.Email;
using IdentityProvider.Infrastructure.Enums;
using IdentityProvider.Infrastructure.Logging.Serilog.Providers;

namespace IdentityProvider.Infrastructure.Certificates.ExpiryValidation
{
    public class CertificateExpirationValidator : ICertificateExpirationValidator
    {
        #region Ctor

        public CertificateExpirationValidator(
            IErrorLogService errorLog,
            IEmailSender emailSender,
            IApplicationConfiguration applicationConfiguration
        )
        {
            _errorLog = errorLog;
            _emailSender = emailSender;
            _applicationConfiguration = applicationConfiguration;

            if (_errorLog == null) throw new ArgumentNullException(nameof(errorLog));
            if (_emailSender == null) throw new ArgumentNullException(nameof(emailSender));
            if (_applicationConfiguration == null) throw new ArgumentNullException(nameof(applicationConfiguration));
        }

        #endregion Ctor

        #region Public Properties

        public CertificateValidationReponse Validate(X509Certificate2 certificate, CertificateTypeEnum application)
        {
            if (certificate == null) throw new ArgumentNullException(nameof(certificate));

            var returnValue = new CertificateValidationReponse
            {
                Success = false
            };

            var willExpireOn = Convert.ToDateTime(certificate.GetExpirationDateString());
            var daysLeftUntilCertificateExpires = (willExpireOn - DateTime.Today).Days;
            var certificateExpiryMessage = string.Empty;

            certificateExpiryMessage = string.Format("Expiry", willExpireOn);

            if (daysLeftUntilCertificateExpires < 60)
            {
                _errorLog.LogWarning(this, certificateExpiryMessage);

                // TODO: mail goes to multiple people in for each or something...
                if (_applicationConfiguration.ShouldSendEmailWhenCertificateExpiryDateValidationFails())
                    _emailSender.SendMailToAdminsAsync(AdminEmail.Bruno, "Pending certificate expiry",
                        certificateExpiryMessage);
            }

            if (daysLeftUntilCertificateExpires < 40)
                _errorLog.LogFatal(this, certificateExpiryMessage);

            return returnValue;
        }

        #endregion Public Properties

        #region Private Properties

        private readonly IErrorLogService _errorLog;
        private readonly IEmailSender _emailSender;
        private readonly IApplicationConfiguration _applicationConfiguration;

        #endregion Private Properties
    }
}