using System.Security.Cryptography.X509Certificates;

namespace Module.CrossCutting.Certificates.FromStore
{
    public interface ICertificateFromStoreProvider
    {
        X509Certificate2 GetValidCertificateFromStoreByThumbprint(StoreLocation storeLocation, string thumbprint);
    }
}