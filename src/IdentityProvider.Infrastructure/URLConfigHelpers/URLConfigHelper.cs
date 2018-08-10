using System;
using System.Web;

namespace IdentityProvider.Infrastructure.URLConfigHelpers
{
    public class UrlConfigHelper
    {
        private static readonly Guid rootKey = Guid.NewGuid();

        public static string GetRoot()
        {
            if (HttpContext.Current.Items.Contains(rootKey))
                return HttpContext.Current.Items[rootKey] as string;

            lock (HttpContext.Current.Items.SyncRoot)
            {
                if (HttpContext.Current.Items.Contains(rootKey))
                    return HttpContext.Current.Items[rootKey] as string;

                var httpRequest = HttpContext.Current.Request;
                var virtualPath = HttpRuntime.AppDomainAppVirtualPath;
                var baseUrl = string.Format("{0}://{1}{2}", httpRequest.Url.Scheme, httpRequest.Url.Authority,
                    virtualPath);

                if (!baseUrl.EndsWith("/")) baseUrl += "/";

                HttpContext.Current.Items.Add(rootKey, baseUrl);

                return baseUrl;
            }
        }
    }
}