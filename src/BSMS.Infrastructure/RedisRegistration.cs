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
        services.AddSingleton<IConnectionMultiplexer>(multiplexer);

        services.AddStackExchangeRedisCache(redisOptions => {
            var connection = connectionString;
            redisOptions.Configuration = connection;
        });

        services.AddSingleton<ICacheService, CacheService>();
    }
}