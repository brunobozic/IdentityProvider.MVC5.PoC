using System;

namespace IdentityProvider.Infrastructure.Cookies
{
    public interface ICookieProvider
    {
        void Save(string key, string value, DateTime expires);
        string Retrieve(string key);
    }
}