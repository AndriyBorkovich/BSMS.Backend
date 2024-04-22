using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using BSMS.Application.Contracts.Caching;
using System.Collections.Concurrent;
using LinqKit;

namespace BSMS.Infrastructure.Caching;

public class CacheService<T>(IDistributedCache cache) : ICacheService<T>
    where T : class
{
    private static readonly ConcurrentDictionary<string, bool> CacheKeys = new();

    public async Task SetRecordAsync(string key, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null, CancellationToken cancellationToken = default)
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

    public async Task<T?> GetRecordAsync(string key, CancellationToken cancellationToken = default)
    {
        var jsonData = await cache.GetStringAsync(key);

        if (jsonData is null)
        {
            return default;
        }

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
}
