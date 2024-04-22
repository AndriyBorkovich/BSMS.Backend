using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using BSMS.Application.Contracts.Caching;
using System.Collections.Concurrent;
using StackExchange.Redis;

namespace BSMS.Infrastructure.Caching;

public class CacheService(IDistributedCache cache, IConnectionMultiplexer connectionMultiplexer) : ICacheService
{
    private static readonly ConcurrentDictionary<string, bool> CacheKeys = new();

    public async Task SetRecordAsync<T>(string key, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null, CancellationToken cancellationToken = default)
        where T: class
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60),
            SlidingExpiration = slidingExpireTime
        };

        var jsonData = JsonSerializer.Serialize(data);
        await cache.SetStringAsync(key, jsonData, options, cancellationToken);

        CacheKeys.TryAdd(key, false);
    }

    public async Task<T?> GetRecordAsync<T>(string key, CancellationToken cancellationToken = default)
        where T: class
    {
        var jsonData = await cache.GetStringAsync(key, cancellationToken);

        if (jsonData is null)
        {
            return default;
        }

        CacheKeys.TryAdd(key, false);
        return JsonSerializer.Deserialize<T>(jsonData);
    }

    public async Task RemoveRecordAsync(string key, CancellationToken cancellationToken = default)
    {
        await cache.RemoveAsync(key, cancellationToken);

        CacheKeys.TryRemove(key, out var _);
    }

    public async Task RemoveRecordsByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default)
    {
        IEnumerable<Task> tasks = CacheKeys.Keys
                                    .Where(k => k.StartsWith(prefixKey))
                                    .Select(k => RemoveRecordAsync(k, cancellationToken));

        await Task.WhenAll(tasks);
    }

    /// <summary>
    /// deletes all keys and their values from cache store
    /// </summary>
    /// <returns></returns>
    public async Task<bool> DeleteAllKeys() 
    {
        if (connectionMultiplexer.IsConnected)
        {
            var server = connectionMultiplexer.GetServer(connectionMultiplexer.GetEndPoints().First());

            await server.FlushAllDatabasesAsync();

            return true;
        }

        return false;
    }
}
