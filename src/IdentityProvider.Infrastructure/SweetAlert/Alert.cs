namespace IdentityProvider.Infrastructure.SweetAlert
{
    public class Alert
    {
        public const string TempDataKey = "TempDataAlerts";

        public Alert(string command, string message)
        {
            Command = command;
            Message = message;
        }

        public string AlertStyle { get; set; }

        public bool Dismissable { get; set; }
        public string Message { get; set; }

        public string Command { get; set; }
    }

    public static class AlertStyles
    {
        public const string Success = "success";
        public const string Information = "info";
        public const string Warning = "warning";
        public const string Danger = "danger";
    }
}