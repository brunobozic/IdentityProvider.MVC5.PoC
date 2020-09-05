using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IdentityProvider.Infrastructure.Email
{
    public class GmailEmailService : IIdentityMessageService
    {
        private readonly SmtpConfiguration _config;

        private const string GmailUserNameKey = "GmailUserName";
        private const string GmailPasswordKey = "GmailPassword";
        private const string GmailHostKey = "GmailHost";
        private const string GmailPortKey = "GmailPort";
        private const string GmailSslKey = "GmailSsl";
        private const string GmailSslPortKey = "GmailSslPort";

        public GmailEmailService()
        {
            _config = new SmtpConfiguration();

            var gmailUserName = ConfigurationManager.AppSettings[GmailUserNameKey];
            var gmailPassword = ConfigurationManager.AppSettings[GmailPasswordKey];
            var gmailHost = ConfigurationManager.AppSettings[GmailHostKey];
            var gmailPort = Int32.Parse(ConfigurationManager.AppSettings[GmailPortKey]);
            var gmailSsl = Boolean.Parse(ConfigurationManager.AppSettings[GmailSslKey]);
            var gmailSslPort = Int32.Parse(ConfigurationManager.AppSettings[GmailSslPortKey]);

            _config.Username = gmailUserName;
            _config.Password = gmailPassword;
            _config.Host = gmailHost;
            _config.Port = gmailPort;
            _config.Ssl = gmailSsl;
            _config.SslPort = gmailSslPort;
        }

        private bool SendEmailMessage(EmailMessage message)
        {
            var success = false;

            try
            {
                if (_config.Ssl)
                {
                    // Credentials
                    var credentials = new NetworkCredential(_config.Username, _config.Password);
                    // Mail message
                    var smtpMessage = new MailMessage()
                    {
                        Subject = message.Subject,
                        Body = message.Body,
                        IsBodyHtml = message.IsHtml,
                        From = new MailAddress("md.authentication@gmail.com")
                    };

                    smtpMessage.To.Add(new MailAddress(message.ToEmail));

                    var smtp = new SmtpClient
                    {
                        Host = _config.Host,
                        Port = _config.SslPort,
                        EnableSsl = _config.Ssl, // 587
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = credentials
                    };

                    smtp.Send(smtpMessage);

                }
                //else
                //{
                //    var smtp = new SmtpClient
                //    {
                //        Host = _config.Host ,
                //        Port = _config.Port ,
                //        EnableSsl = _config.Ssl ,
                //        DeliveryMethod = SmtpDeliveryMethod.Network ,
                //        UseDefaultCredentials = false ,
                //        Credentials = new NetworkCredential(_config.Username , _config.Password)
                //    };
                //    using (var smtpMessage = new MailMessage(_config.Username , message.ToEmail))
                //    {
                //        smtpMessage.Subject = message.Subject;
                //        smtpMessage.Body = message.Body;
                //        smtpMessage.IsBodyHtml = message.IsHtml;
                //        smtp.Send(smtpMessage);
                //    }
                //}

                success = true;
            }
            catch (Exception ex)
            {
                //todo: add logging integration
                //throw;
                return success;
            }

            return success;
        }

        public void SendMail(string @from, string to, string subject, string body)
        {
            if (string.IsNullOrEmpty(@from)) throw new ArgumentNullException(nameof(@from));
            if (string.IsNullOrEmpty(to)) throw new ArgumentNullException(nameof(to));
            if (string.IsNullOrEmpty(subject)) throw new ArgumentNullException(nameof(subject));
            if (string.IsNullOrEmpty(body)) throw new ArgumentNullException(nameof(body));

            var message = new EmailMessage
            {
                Body = body,
                Subject = subject,
                ToEmail = to,
                From = from
            };

            SendEmailMessage(message);
        }

        public Task SendAsync(IdentityMessage message)
        {

            if (string.IsNullOrEmpty(message.Destination)) throw new ArgumentNullException(nameof(message.Destination));
            if (string.IsNullOrEmpty(message.Subject)) throw new ArgumentNullException(nameof(message.Subject));
            if (string.IsNullOrEmpty(message.Body)) throw new ArgumentNullException(nameof(message.Body));

            var myMessage = new EmailMessage
            {
                Body = message.Body,
                Subject = message.Subject,
                ToEmail = message.Destination,
                From = "md.authentication@gmail.com"
            };

            SendEmailMessage(myMessage);

            return Task.FromResult(0);

        }
    }

    public class EmailMessage
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public string From { get; set; }
    }

    internal class SmtpConfiguration
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool Ssl { get; set; }
        public int SslPort { get; set; }
    }
}
