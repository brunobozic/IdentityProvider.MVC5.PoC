using System;

namespace Module.CrossCutting.Cookies
{
    public interface ICookieProvider
    {
        void Save(string key, string value, DateTime expires);
        string Retrieve(string key);
    }
}