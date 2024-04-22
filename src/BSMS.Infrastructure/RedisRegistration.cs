using BSMS.Application.Contracts.Caching;
using BSMS.Infrastructure.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BSMS.Infrastructure;

public static class RedisRegistration
{
    public static void AddRedisCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(redisOptions => {
            var connection = configuration.GetConnectionString("Redis");
            redisOptions.Configuration = connection;
        });

        services.AddSingleton(typeof(ICacheService<>), typeof(CacheService<>));
    }
}