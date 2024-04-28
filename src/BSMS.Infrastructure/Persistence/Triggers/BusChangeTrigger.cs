using BSMS.Application.Contracts.Caching;
using BSMS.Application.Helpers;
using BSMS.Core.Entities;
using EntityFrameworkCore.Triggered;

namespace BSMS.Infrastructure.Persistence.Triggers;

public class BusChangeTrigger(ICacheService cacheService) : IAfterSaveTrigger<Bus>
{
    public async Task AfterSave(ITriggerContext<Bus> context, CancellationToken cancellationToken)
    {
        await cacheService.RemoveRecordsByPrefixAsync(CachePrefixConstants.BusesKey, cancellationToken);
    }
}