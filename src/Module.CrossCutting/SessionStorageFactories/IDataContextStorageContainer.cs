namespace Module.CrossCutting.SessionStorageFactories
{
    public interface IDataContextStorageContainer<T>
    {
        T GetDataContext();
        void Store(T objectContext);
        void Clear();
    }
}