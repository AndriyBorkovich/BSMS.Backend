using BSMS.Application.Contracts.Caching;
using BSMS.Infrastructure.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace BSMS.Infrastructure;

public static class RedisRegistration
{
    public static void AddRedisCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Redis");
        
        var multiplexer = ConnectionMultiplexer.Connect(connectionString);
        
        if(multiplexer.IsConnected)
        {
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);

            services.AddStackExchangeRedisCache(redisOptions => {
                redisOptions.Configuration = connectionString;
            });

            services.AddSingleton<ICacheService, CacheService>();
        }
        else
        {
            // injecting service which simulates cache work (literraly it doesn't do anything)
            services.AddSingleton<ICacheService, FakeCacheService>();
        }
    }
}