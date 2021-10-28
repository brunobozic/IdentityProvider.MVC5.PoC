using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using IdentityProvider.Infrastructure.ApplicationConfiguration;
using IdentityProvider.Infrastructure.Certificates.FromEmbeddedResource;
using IdentityProvider.Infrastructure.Certificates.FromStore;
using IdentityProvider.Infrastructure.ConfigurationProvider;
using IdentityProvider.Infrastructure.Email;
using IdentityProvider.Infrastructure.Enums;
using IdentityProvider.Infrastructure.Logging.Serilog.Providers;

namespace IdentityProvider.Infrastructure.Certificates.Manager
{
    public class CertificateManager : ICertificateManager
    {
        #region Ctor

        public CertificateManager(
            ICertificateFromStoreProvider certificateProvider
            , ICertificateFromEmbededResourceProvider embeddedResourceCertificateProvider
            , IErrorLogService errorLog
            , IEmailSender emailSender
            , IConfigurationProvider configurationRepository
            , IApplicationConfiguration applicationConfiguration
            , ICertificateFromStoreProvider certificateFromStoreProvider
        )
        {
            _configurationRepository = configurationRepository;
            _certificateProvider = certificateProvider;
            _errorLog = errorLog;
            _emailSender = emailSender;
            _embeddedResourceCertificateProvider = embeddedResourceCertificateProvider;
            _applicationConfiguration = applicationConfiguration;
            _certificateFromStoreProvider = certificateFromStoreProvider;

            if (_certificateProvider == null) throw new ArgumentNullException(nameof(certificateProvider));
            if (_emailSender == null) throw new ArgumentNullException(nameof(emailSender));
            if (_errorLog == null) throw new ArgumentNullException(nameof(errorLog));
            if (_configurationRepository == null) throw new ArgumentNullException(nameof(configurationRepository));
            if (_embeddedResourceCertificateProvider == null)
                throw new ArgumentNullException(nameof(embeddedResourceCertificateProvider));
            if (_applicationConfiguration == null) throw new ArgumentNullException(nameof(applicationConfiguration));
            if (_certificateFromStoreProvider == null)
                throw new ArgumentNullException(nameof(certificateFromStoreProvider));
        }

        #endregion Ctor

        #region Public methods

        public X509Certificate2 GetCertificateOfType(CertificateTypeEnum certificateType)
        {
            if (takeCertificatesFromLocalMachineStore)
            {
                var myStoreLocation = StoreLocation.LocalMachine;
                var certificateThumbprint = string.Empty;

                certificateThumbprint = _applicationConfiguration.GetAppCertThumbprintByType(certificateType);

                if (string.IsNullOrEmpty(certificateThumbprint))
                    throw new NoNullAllowedException(nameof(certificateThumbprint));

                _errorLog.LogInfo(
                    this,
                    string.Format(ReceivedApplicationThumbprintFromProviderDebugMessage,
                        certificateThumbprint)
                );

                return _certificateProvider.GetValidCertificateFromStoreByThumbprint(
                    myStoreLocation,
                    certificateThumbprint
                );
            }

            return _embeddedResourceCertificateProvider.GetValidCertificateFromEmbeddedResource();
        }

        #endregion Public methods

        #region Private properties

        private readonly ICertificateFromStoreProvider _certificateProvider;
        private readonly IEmailSender _emailSender;
        private readonly ICertificateFromEmbededResourceProvider _embeddedResourceCertificateProvider;
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly ICertificateFromStoreProvider _certificateFromStoreProvider;
        private readonly IConfigurationProvider _configurationRepository;
        private readonly IErrorLogService _errorLog;
        private readonly bool takeCertificatesFromLocalMachineStore = false; // might use this one at a later date...

        #endregion Private properties

        #region Private methods

        #endregion Private methods

        #region Localization messages

        private const string ReceivedApplicationThumbprintFromProviderDebugMessage =
            "Recieved application certificate thumbrint from provider: [ {0} ]";

        private const string ReceivedValidationThumbprintFromProviderDebugMessage =
            "Recieved validation certificate thumbrint from provider: [ {0} ]";

        private const string FoundMatchingApplicationCertificateMessage =
            "Found a matching valid application certificate by thumbprint: [ {0} ] from store: [ {1} ], with subject: [ {2} ] and expiration at [ {3} ]";

        private const string FoundMatchingValidationCertificateMessage =
            "Found a matching valid validation certificate by thumbprint: [ {0} ] from store: [ {1} ]";

        private const string ApplicationCertificateExpiryWarningMessage =
            "The certificate will expire on: [ {0} ]";

        private const string ValidationCertificateExpiryWarningMessage =
            "The validation certificate will expire on: [ {0} ]";

        private const string ProblemFetchingTestEnvironmentMarkFromConfig =
            "Could not read the configuration property that lets us know we are on test environment, using false if debug build, and using true if release build.";

        #endregion Localization messages
    }
}