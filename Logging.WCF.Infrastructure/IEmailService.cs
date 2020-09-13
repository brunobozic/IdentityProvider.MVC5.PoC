namespace Logging.WCF.Infrastructure
{
    public interface IEmailService
    {
        void SendMail(string from, string to, string subject, string body);
    }
}