using System;

namespace IdentityProvider.Infrastructure.Cookies
{
    public interface ICookieStorageService
    {
        void Save(string key, string value, DateTime expires);
        string Retrieve(string key);
    }
}