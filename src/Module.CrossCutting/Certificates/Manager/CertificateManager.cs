using Module.CrossCutting.Certificates.FromEmbeddedResource;
using Module.CrossCutting.Certificates.FromStore;
using Module.CrossCutting.Email;
using Module.CrossCutting.Enums;
using Module.CrossCutting.Logging.Serilog.Providers;
using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace Module.CrossCutting.Certificates.Manager
{
    public class CertificateManager : ICertificateManager
    {
        #region Ctor

        public CertificateManager(
            ICertificateFromStoreProvider certificateProvider
            , ICertificateFromEmbededResourceProvider embeddedResourceCertificateProvider
            , IErrorLogService errorLog
            , IEmailSender emailSender
            , ICertificateFromStoreProvider certificateFromStoreProvider
        )
        {
            _certificateProvider = certificateProvider;
            _errorLog = errorLog;
            _emailSender = emailSender;
            _embeddedResourceCertificateProvider = embeddedResourceCertificateProvider;
            _certificateFromStoreProvider = certificateFromStoreProvider;

            if (_certificateProvider == null) throw new ArgumentNullException(nameof(certificateProvider));
            if (_emailSender == null) throw new ArgumentNullException(nameof(emailSender));
            if (_errorLog == null) throw new ArgumentNullException(nameof(errorLog));

            if (_embeddedResourceCertificateProvider == null)
                throw new ArgumentNullException(nameof(embeddedResourceCertificateProvider));

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

                // certificateThumbprint = _applicationConfiguration.GetAppCertThumbprintByType(certificateType);

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
        private readonly ICertificateFromStoreProvider _certificateFromStoreProvider;
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