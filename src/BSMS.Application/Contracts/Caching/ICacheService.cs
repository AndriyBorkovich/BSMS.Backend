namespace BSMS.Application.Contracts.Caching;

/// <summary>
/// Redis-compatible caching service
/// </summary>
public interface ICacheService<T> where T : class
{
    Task SetRecordAsync(string key, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null, CancellationToken cancellationToken = default);
    Task<T?> GetRecordAsync(string key, CancellationToken cancellationToken = default);
    Task RemoveRecordAsync(string recordId, CancellationToken cancellationToken = default);
    Task RemoveRecordsByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default);
}