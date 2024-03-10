using Microsoft.AspNetCore.Http;
using Module.CrossCutting.Cookies;

namespace IdentityProvider.ServiceLayer.Services
{
    public class CookieStorageService : ICookieStorageService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieStorageService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Save(string key, string value, DateTime expires)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, new CookieOptions
            {
                Expires = expires
            });
        }

        public string Retrieve(string key)
        {
            var cookie = _httpContextAccessor.HttpContext.Request.Cookies[key];
            return cookie ?? string.Empty;
        }
    }
}
