using __SolutionName__.Application.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace __SolutionName__.Application.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redisConnection;

        public RedisCacheService(IConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var db = _redisConnection.GetDatabase();
            var value = await db.StringGetAsync(key);
            return value.HasValue ? JsonSerializer.Deserialize<T>(value) : default;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            var db = _redisConnection.GetDatabase();
            var serializedValue = JsonSerializer.Serialize(value);
            await db.StringSetAsync(key, serializedValue, expiration);
        }

        public async Task RemoveAsync(string key)
        {
            var db = _redisConnection.GetDatabase();
            await db.KeyDeleteAsync(key);
        }
    }
}
