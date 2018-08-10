using System.Web;

namespace IdentityProvider.Infrastructure.ApplicationContext
{
    public class LoggingContextProvider : IAddLoggingContextProvider
    {
        private ContextDataModel _properties;

        public ContextDataModel GetContextProperties()
        {
            _properties = new ContextDataModel();

            if (HttpContext.Current != null)
            {
                HttpRequest request = null;

                try
                {
                    request = HttpContext.Current.Request;
                }
                catch (HttpException)
                {
                }

                if (request != null)
                {
                    _properties.UserAgent = request.Browser == null ? "" : request.Browser.Browser;
                    _properties.RemoteHost = request.ServerVariables["REMOTE_HOST"];
                    _properties.Path = request.Url.AbsolutePath;
                    _properties.Query = request.Url.Query;
                    _properties.Referrer = request.UrlReferrer == null ? "" : request.UrlReferrer.ToString();
                    _properties.Method = request.HttpMethod;
                }

                var items = HttpContext.Current.Items;

                var requestId = items?["RequestId"];

                if (requestId != null)
                    _properties.RequestId = items["RequestId"].ToString();

                var session = HttpContext.Current.Session;

                var sessionId = session?["SessionId"];

                if (sessionId != null)
                    _properties.SessionId = session["SessionId"].ToString();
            }

            return _properties;
        }

        public string FakeUserNameForTestingPurposes { get; }

        public string GetContextualFullFilePath(string fileName)
        {
#if DEBUG
            return HttpContext.Current.Server.MapPath(string.Concat("~/bin/", fileName));
#endif
#if !DEBUG
            return HttpContext.Current.Server.MapPath(string.Concat("~/", fileName));
#endif
        }

        public string GetUserName()
        {
            var userName = "<N/A>";

            try
            {
                if (HttpContext.Current != null && HttpContext.Current.User != null)
                    userName = HttpContext.Current.User.Identity.IsAuthenticated
                        ? HttpContext.Current.User.Identity.Name
                        : "<N/A>";
            }
            catch
            {
            }

            return userName;
        }
    }
}