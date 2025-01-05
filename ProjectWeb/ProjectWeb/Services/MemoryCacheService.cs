using System.Runtime.Caching;
using ProjectWeb.Interfaces;

namespace ProjectWeb.Services
{
    public class MemoryCacheService : ICacheService
    {
        private readonly ObjectCache _cache = MemoryCache.Default;
        private readonly int _defaultCacheTime = 60;
        private readonly ILogger<MemoryCacheService> _logger;
        public MemoryCacheService(ILogger<MemoryCacheService> logger)
        {
            _logger = logger;
        }
        public T Get<T>(string key)
        {
            if (!_cache.Contains(key))
            {
                _logger.LogWarning("Cheia de cache {CacheKey} nu există.", key);
                return default;
            }

            _logger.LogInformation("Accesare cache pentru cheia {CacheKey}.", key);
            return (T)_cache[key];
        }

        public void Set(string key, object value, int? cacheTime = null)
        {
            if (value == null) return;

            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(cacheTime ?? _defaultCacheTime)
            };

            _cache.Set(key, value, policy);
            _logger.LogInformation("Datele au fost salvate în cache pentru cheia {CacheKey}.", key);

        }

        public bool IsSet(string key)
        {
            return _cache.Contains(key);
        }
        public void Update(string key, object value, int? cacheTime = null)
        {
            if (!_cache.Contains(key)) return;  // Verifică dacă cheia există în cache

            // Șterge vechea valoare din cache
            _cache.Remove(key);

            // Adaugă noua valoare în cache
            Set(key, value, cacheTime);
        }
        public void Remove(string key)
        {
            _cache.Remove(key);
            _logger.LogInformation("Cache-ul pentru cheia {CacheKey} a fost șters.", key);

        }

        public void Clear()
        {
            foreach (var item in _cache)
            {
                _cache.Remove(item.Key);
            }
        }
    }
}
