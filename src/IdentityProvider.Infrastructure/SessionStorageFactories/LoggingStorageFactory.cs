using System.Web;

namespace IdentityProvider.Infrastructure.SessionStorageFactories
{
    public static class LoggingStorageFactory<T> where T : class
    {
        private static ILoggingStorageContainer<T> _dataContextStorageContainer;

        public static ILoggingStorageContainer<T> CreateStorageContainer()
        {
            if (_dataContextStorageContainer == null)
                if (HttpContext.Current == null)
                    _dataContextStorageContainer = new ThreadLoggingStorageContainer<T>();
                else
                    _dataContextStorageContainer = new HttpLoggingStorageContainer<T>();
            return _dataContextStorageContainer;
        }
    }

    public interface ILoggingStorageContainer<T>
    {
        T GetLogger();
        void Store(T logger);
        void Clear();
    }
}