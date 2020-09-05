using System.Web;

namespace IdentityProvider.Infrastructure.SessionStorageFactories
{
    public class HttpDataContextStorageContainer<T> :
        IDataContextStorageContainer<T> where T : class
    {
        private const string DataContextKey = "DataContext";

        public T GetDataContext()
        {
            T objectContext = null;
            if (HttpContext.Current.Items.Contains(DataContextKey))
                objectContext = (T)HttpContext.Current.Items[DataContextKey];
            return objectContext;
        }

        public void Clear()
        {
            if (HttpContext.Current.Items.Contains(DataContextKey))
                HttpContext.Current.Items[DataContextKey] = null;
        }

        public void Store(T objectContext)
        {
            if (HttpContext.Current.Items.Contains(DataContextKey))
                HttpContext.Current.Items[DataContextKey] = objectContext;
            else
                HttpContext.Current.Items.Add(DataContextKey, objectContext);
        }
    }
}