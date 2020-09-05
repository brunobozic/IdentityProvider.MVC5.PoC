using IdentityProvider.Infrastructure.Enums;
using System.Security.Cryptography.X509Certificates;

namespace IdentityProvider.Infrastructure.Certificates.ExpiryValidation
{
    public interface ICertificateExpirationValidator
    {
        CertificateValidationReponse Validate(X509Certificate2 certificate, CertificateTypeEnum application);
    }
}