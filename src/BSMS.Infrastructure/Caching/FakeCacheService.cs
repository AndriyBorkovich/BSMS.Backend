using BSMS.Application.Contracts.Caching;

namespace BSMS.Infrastructure.Caching;
public class FakeCacheService : ICacheService
{
    public Task<bool> DeleteAllKeys()
    {
        return Task.FromResult(true);
    }

    public Task<T?> GetRecordAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        return Task.FromResult(default(T));
    }

    public Task RemoveRecordAsync(string recordId, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task RemoveRecordsByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task SetRecordAsync<T>(string key, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null, CancellationToken cancellationToken = default) where T : class
    {
        return Task.CompletedTask;
    }
}