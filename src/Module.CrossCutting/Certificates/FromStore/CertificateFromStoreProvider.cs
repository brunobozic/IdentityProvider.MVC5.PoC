using Module.CrossCutting.Logging.Serilog.Providers;
using System.Data;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Module.CrossCutting.Certificates.FromStore
{
    public class CertificateFromStoreProvider : ICertificateFromStoreProvider
    {
        #region Private properties

        private readonly IErrorLogService _log;

        #endregion Private properties

        #region Public properties

        public X509Certificate2 GetValidCertificateFromStoreByThumbprint(StoreLocation storeLocation, string thumbprint)
        {
            if (string.IsNullOrEmpty(thumbprint))
                throw new ArgumentNullException(nameof(thumbprint));

            var certificateThumbprint = Regex.Replace(thumbprint, @"[^\da-zA-z]", string.Empty).ToUpper();
            var certificateStore = new X509Store(storeLocation);

            X509Certificate2Collection certificatesFound = null;

            try
            {
                certificateStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                var certificateCollection = certificateStore.Certificates;
                certificatesFound =
                    certificateCollection.Find(X509FindType.FindByThumbprint, certificateThumbprint, true);
            }
            catch (Exception certificateStoreOperationException)
            {
                _log.LogFatal(certificateStoreOperationException,
                    string.Format(CertificateStoreExceptionErrorMessage, thumbprint));
            }
            finally
            {
                certificateStore.Close();
            }

            if (certificatesFound == null)
                throw new NoNullAllowedException(nameof(certificatesFound));

            if (certificatesFound.Count == 0)
                throw new CryptographicException(string.Format(NoCertificateFoundErrorMessage, thumbprint));

            return certificatesFound[0];
        }

        #endregion Public properties

        #region Ctor

        public CertificateFromStoreProvider(IErrorLogService logger)
        {
            _log = logger;
        }

        public CertificateFromStoreProvider()
        {
        }

        #endregion Ctor

        #region Localizable resources

        public const string NoCertificateFoundErrorMessage =
            "No certificate found by the requested thumbprint of: [ {0} ]";

        public const string CertificateStoreExceptionErrorMessage =
            "Error fetching from certificate store by the requested thumbprint of: [ {0} ]";

        #endregion Localizable resources
    }
}