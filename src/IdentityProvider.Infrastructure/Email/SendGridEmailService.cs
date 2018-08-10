using System;
using System.Threading.Tasks;
using IdentityProvider.Infrastructure.ApplicationConfiguration;
using IdentityProvider.Infrastructure.Logging.Log4Net;
using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace IdentityProvider.Infrastructure.Email
{
    public class SEndGridEmailService : IIdentityMessageService
    {
        private readonly IApplicationConfiguration _configurationRepository;
        private readonly ILog4NetLoggingService _loggingService;

        public SEndGridEmailService(
            ILog4NetLoggingService loggingService
            , IApplicationConfiguration configurationRepository)
        {
            _loggingService = loggingService;
            _configurationRepository = configurationRepository;
        }

        public async Task SendAsync(IdentityMessage message)
        {
            await ConfigSendGridasync(message);
        }

        // Use NuGet to install SendGrid (Basic C# client lib) 
        private async Task<Response> ConfigSendGridasync(IdentityMessage message)
        {
            Response response = null;
            //var myMessage = new SendGridMessage
            //{
            //    From = new System.Net.Mail.MailAddress("IdentityProviderPoc@gmail.com", "Identity Provider"),
            //    Subject = message.Subject,
            //    Text = message.Body,
            //    Html = message.Body
            //};
            //myMessage.AddTo(message.Destination);

            var apiKey = _configurationRepository.GetSendGridApiKey();
            var mailPassword = _configurationRepository.GetMailPassword();

            //var apiKey = _configurationRepository.GetConfigurationValue<string>("SendGridApiKey");
            //var mailPassword = _configurationRepository.GetConfigurationValue<string>("MailPassword");

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("IdentityProviderPoc@gmail.com", "Identity Provider");
            var subject = message.Subject;
            var to = new EmailAddress(message.Destination);
            var plainTextContent = message.Body;
            var htmlContent = message.Body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);


            try
            {
                response = await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                _loggingService.LogFatal(this, "", ex);
            }

            return response;

            //var credentials = new NetworkCredential(
            //    mailAccount,
            //    mailPassword
            //);

            //// Create a Web transport for sending email.
            //var transportWeb = new Web(credentials);

            //// Send the email.
            //if (transportWeb != null)
            //{
            //    await transportWeb.DeliverAsync(myMessage);
            //}
            //else
            //{
            //    Trace.TraceError("Failed to create Web transport.");
            //    await Task.FromResult(0);
            //}
        }
    }
}