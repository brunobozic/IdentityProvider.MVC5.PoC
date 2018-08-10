using System.Security.Cryptography.X509Certificates;

namespace IdentityProvider.Infrastructure.Certificates.FromEmbeddedResource
{
    public interface ICertificateFromEmbededResourceProvider
    {
        X509Certificate2 GetValidCertificateFromEmbeddedResource();
    }
}