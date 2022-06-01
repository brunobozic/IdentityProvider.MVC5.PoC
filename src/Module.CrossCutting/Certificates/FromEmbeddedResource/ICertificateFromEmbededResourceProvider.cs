using System.Security.Cryptography.X509Certificates;

namespace Module.CrossCutting.Certificates.FromEmbeddedResource
{
    public interface ICertificateFromEmbededResourceProvider
    {
        X509Certificate2 GetValidCertificateFromEmbeddedResource();
    }
}