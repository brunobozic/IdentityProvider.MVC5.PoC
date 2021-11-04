using System.Text;

namespace IdentityProvider.Infrastructure.Email
{
    public class TextLoggingEmailService : IEmailService
    {
        public void SendMail(string from, string to, string subject, string body)
        {
            var email = new StringBuilder();

            email.AppendLine($"To: {to}");
            email.AppendLine($"From: {from}");
            email.AppendLine($"Subject: {subject}");
            email.AppendLine($"Body: {body}");

            // Log4NetLoggingFactory.GetLogger().LogInfo(this, email.ToString());
        }
    }
}