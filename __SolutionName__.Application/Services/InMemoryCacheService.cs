using __SolutionName__.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace __SolutionName__.Application.Services
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task<T?> GetAsync<T>(string key)
        {
            return Task.FromResult(_memoryCache.TryGetValue(key, out T value) ? value : default);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            _memoryCache.Set(key, value, expiration);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            _memoryCache.Remove(key);
            return Task.CompletedTask;
        }
    }
}
