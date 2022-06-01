namespace Module.CrossCutting.Caching
{
    public interface IMemoryCacheProvider
    {
        void Clear(string key);
        T Get<T>(string key);
        void Save<T>(T item, string key);
        void Remove<T>(string key);
    }
}