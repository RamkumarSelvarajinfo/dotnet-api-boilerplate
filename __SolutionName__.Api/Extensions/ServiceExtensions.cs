using __SolutionName__.Application.Interfaces;
using __SolutionName__.Application.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace __SolutionName__.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("RedisConnection");
            });

            // Register caching services
            if (configuration.GetValue<bool>("UseRedis"))
            {
                services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")));
                services.AddSingleton<ICacheService, RedisCacheService>();
            }
            else
            {
                services.AddMemoryCache();
                services.AddSingleton<ICacheService, InMemoryCacheService>();
            }

            return services;
        }
    }
}
