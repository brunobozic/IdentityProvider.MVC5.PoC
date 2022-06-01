using Module.CrossCutting.Enums;
using System.Security.Cryptography.X509Certificates;

namespace Module.CrossCutting.Certificates.ExpiryValidation
{
    public interface ICertificateExpirationValidator
    {
        CertificateValidationReponse Validate(X509Certificate2 certificate, CertificateTypeEnum application);
    }
}