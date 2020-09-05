using System;
using System.Collections;
using System.Threading;

namespace IdentityProvider.Infrastructure.SessionStorageFactories
{
    public class ThreadLoggingStorageContainer<T> :
        ILoggingStorageContainer<T> where T : class
    {
        private static readonly Hashtable StoredContexts = new Hashtable();

        public void Clear()
        {
            if (StoredContexts.Contains(GetThreadName()))
                StoredContexts[GetThreadName()] = null;
        }

        public T GetLogger()
        {
            T context = null;

            if (StoredContexts.Contains(GetThreadName()))
                context = (T)StoredContexts[GetThreadName()];
            return context;
        }

        public void Store(T objectContext)
        {
            if (StoredContexts.Contains(GetThreadName()))
                StoredContexts[GetThreadName()] = objectContext;
            else
                StoredContexts.Add(GetThreadName(), objectContext);
        }

        private static string GetThreadName()
        {
            if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = Guid.NewGuid().ToString();
            return Thread.CurrentThread.Name;
        }
    }

    public interface IThreadLoggingStorageContainer<T>
    {
    }
}