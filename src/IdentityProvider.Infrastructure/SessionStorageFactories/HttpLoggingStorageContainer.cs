using System.Web;

namespace IdentityProvider.Infrastructure.SessionStorageFactories
{
    public class HttpLoggingStorageContainer<T> :
        ILoggingStorageContainer<T> where T : class
    {
        private const string StorageKey = "Logging";

        public T GetLogger()
        {
            T objectContext = null;
            if (HttpContext.Current.Items.Contains(StorageKey))
                objectContext = (T)HttpContext.Current.Items[StorageKey];
            return objectContext;
        }

        public void Clear()
        {
            if (HttpContext.Current.Items.Contains(StorageKey))
                HttpContext.Current.Items[StorageKey] = null;
        }

        public void Store(T objectContext)
        {
            if (HttpContext.Current.Items.Contains(StorageKey))
                HttpContext.Current.Items[StorageKey] = objectContext;
            else
                HttpContext.Current.Items.Add(StorageKey, objectContext);
        }
    }
}