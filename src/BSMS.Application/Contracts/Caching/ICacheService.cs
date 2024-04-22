namespace BSMS.Application.Contracts.Caching;

/// <summary>
/// Redis-compatible caching service
/// </summary>
public interface ICacheService
{
    Task SetRecordAsync<T>(string key, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null, CancellationToken cancellationToken = default)
        where T: class;
    Task<T?> GetRecordAsync<T>(string key, CancellationToken cancellationToken = default)
        where T: class;
    Task RemoveRecordAsync(string recordId, CancellationToken cancellationToken = default);
    Task RemoveRecordsByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default);
    Task<bool> DeleteAllKeys();
}