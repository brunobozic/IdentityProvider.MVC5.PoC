using System.Web.Caching;

namespace IdentityProvider.Infrastructure.Caching
{
    public class MemoryCacheProvider : IMemoryCacheProvider
    {
        private const string cacheName = "NiasKeyValueSessionCache";
        private Cache _internalMemoryCache;

        public void Clear(string key)
        {
            Initialize();
            _internalMemoryCache.Remove(key);
        }

        public T Get<T>(string key)
        {
            Initialize();
            return (T)_internalMemoryCache.Get(key);
        }

        public void Save<T>(T item, string key)
        {
            Initialize();
            if (_internalMemoryCache.Get(key) == null)
                _internalMemoryCache.Insert(key, item);
        }

        public void Remove<T>(string key)
        {
            Initialize();
            if (_internalMemoryCache.Get(key) != null)
                _internalMemoryCache.Remove(key);
        }


        public void Initialize()
        {
            if (_internalMemoryCache == null) _internalMemoryCache = new Cache();
        }
    }
}