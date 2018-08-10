using System;
using System.Web;

namespace IdentityProvider.Infrastructure.ApplicationContext
{
    public class HttpContextProvider : IContextProvider
    {
        private ContextDataModel _properties = new ContextDataModel();

        public HttpContextProvider()
        {
            Disposed = false;
            if (HttpContext.Current == null)
                throw new ArgumentException("There's no available Http context.");
        }

        public bool Disposed { get; set; }

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

        public string FakeUserNameForTestingPurposes
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public string FakeGetContextualFullFilePath
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public void Dispose()
        {
            _properties = null;
            Disposed = true;
        }
    }


    public class FakeHttpContextService : IContextProvider
    {
        public FakeHttpContextService()
        {
            Disposed = false;
            if (HttpContext.Current == null)
                throw new ArgumentException("There's no available Http context.");
        }

        public bool Disposed { get; set; }

        public string FakeGetContextualFullFilePath { get; set; } = @"FileNameForTestingPurposes";

        public string FakeUserNameForTestingPurposes { get; set; } = "TestUser";

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
            var userName = "FakeHttpContextService UserName";

            try
            {
                if (HttpContext.Current != null && HttpContext.Current.User != null)
                    userName = HttpContext.Current.User.Identity.IsAuthenticated
                        ? HttpContext.Current.User.Identity.Name
                        : "FakeHttpContextService UserName";
            }
            catch
            {
            }

            return userName;
        }

        public ContextDataModel GetContextProperties()
        {
            // Fake data for testing purposes...
            var props = new ContextDataModel
            {
                UserAgent = "FakeHttpContextService UserAgent",
                RemoteHost = "FakeHttpContextService RemoteHost",
                Path = "FakeHttpContextService Path",
                Query = "FakeHttpContextService Query",
                Referrer = "FakeHttpContextService Referrer",
                Method = "FakeHttpContextService Method"
            };

            return props;
        }

        public void Dispose()
        {
            Disposed = true;
        }
    }
}