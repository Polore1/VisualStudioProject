namespace ProjectWeb.Interfaces
{
    public interface ICacheService
    {
        T Get<T>(string key);
        void Set(string key, object value, int? cacheTime = null);
        bool IsSet(string key);
        void Remove(string key);
        void Clear();
    }
}
