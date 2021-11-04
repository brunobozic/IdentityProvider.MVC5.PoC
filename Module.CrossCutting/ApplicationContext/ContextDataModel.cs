namespace IdentityProvider.Infrastructure.ApplicationContext
{
    public class ContextDataModel
    {
        private const string NotAvailable = "N/A";

        public ContextDataModel()
        {
            UserAgent = NotAvailable;
            RemoteHost = NotAvailable;
            Path = NotAvailable;
            Query = NotAvailable;
            Referrer = NotAvailable;
            RequestId = NotAvailable;
            SessionId = NotAvailable;
        }

        public string UserAgent { get; set; }
        public string RemoteHost { get; set; }
        public string Path { get; set; }
        public string Query { get; set; }
        public string Referrer { get; set; }
        public string RequestId { get; set; }
        public string SessionId { get; set; }
        public string Method { get; set; }
    }
}