using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGridMessage = SendGrid.Helpers.Mail.SendGridMessage;

namespace IdentityProvider.Infrastructure.Email
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(AuthMessageSenderOptions optionsAccessor)
        {
            Options = optionsAccessor;
        }

        public EmailSender()
        {
        }

        public AuthMessageSenderOptions Options { get; }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }

        public Task SendMailToAdminsAsync(string email, string subject, string certificateExpiryMessage)
        {
            return Execute(Options.SendGridKey, subject, certificateExpiryMessage, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage
            {
                From = new EmailAddress("niasPoCApplication@NiasPoc.com", "Test System"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));
            return client.SendEmailAsync(msg);
        }
    }
}