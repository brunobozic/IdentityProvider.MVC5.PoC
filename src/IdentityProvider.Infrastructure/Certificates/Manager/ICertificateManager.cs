﻿using System.Security.Cryptography.X509Certificates;
using IdentityProvider.Infrastructure.Enums;

namespace IdentityProvider.Infrastructure.Certificates.Manager
{
    public interface ICertificateManager
    {
        /// <summary>
        ///     Returns the X509Certificate2 by type (possible types: Production, Validation)
        /// </summary>
        /// <param name="certificateType"></param>
        /// <returns></returns>
        X509Certificate2 GetCertificateOfType(CertificateTypeEnum certificateType);
    }
}