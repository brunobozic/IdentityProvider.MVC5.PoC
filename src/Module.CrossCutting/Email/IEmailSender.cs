using System.Threading.Tasks;

namespace Module.CrossCutting.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendMailToAdminsAsync(string email, string subject, string certificateExpiryMessage);
    }
}