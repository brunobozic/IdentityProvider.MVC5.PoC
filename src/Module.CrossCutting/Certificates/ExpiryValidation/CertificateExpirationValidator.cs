﻿using Module.CrossCutting.Email;
using Module.CrossCutting.Enums;
using Serilog;
using System.Security.Cryptography.X509Certificates;

namespace Module.CrossCutting.Certificates.ExpiryValidation
{
    public class CertificateExpirationValidator : ICertificateExpirationValidator
    {
        #region Ctor

        public CertificateExpirationValidator(

            IEmailSender emailSender
        )
        {

            _emailSender = emailSender;



            if (_emailSender == null) throw new ArgumentNullException(nameof(emailSender));
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
                Log.Warning(certificateExpiryMessage);

            //// TODO: mail goes to multiple people in for each or something...
            //if (_applicationConfiguration.ShouldSendEmailWhenCertificateExpiryDateValidationFails())
            //    _emailSender.SendMailToAdminsAsync(AdminEmail.Bruno, "Pending certificate expiry",
            //        certificateExpiryMessage);

            if (daysLeftUntilCertificateExpires < 40)
                Log.Fatal(certificateExpiryMessage);

            return returnValue;
        }

        #endregion Public Properties

        #region Private Properties


        private readonly IEmailSender _emailSender;

        #endregion Private Properties
    }
}