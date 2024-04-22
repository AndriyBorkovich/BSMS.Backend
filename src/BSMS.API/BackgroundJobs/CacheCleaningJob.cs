
using BSMS.Application.Contracts.Caching;

namespace BSMS.API.BackgroundJobs;

/// <summary>
/// Cleans up all previous cache data before app starts
/// </summary>
/// <param name="scopeFactory"></param>
public class CacheCleaningJob(
    IServiceScopeFactory scopeFactory,
    ILogger<CacheCleaningJob> logger) : IHostedService
{
    /// <inheritdoc/>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();

        logger.LogInformation("Cache cleaning job has started...");
        var result = await cacheService.DeleteAllKeys();
        if(result) 
        {
            logger.LogInformation("Previous cache was cleaned");
        }
        else
        {
            logger.LogWarning("Cache was not cleaned");
        }
    }

    /// <inheritdoc/>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}