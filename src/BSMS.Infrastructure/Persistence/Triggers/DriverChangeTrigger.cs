using BSMS.Application.Contracts.Caching;
using BSMS.Application.Helpers;
using BSMS.Core.Entities;
using EntityFrameworkCore.Triggered;

namespace BSMS.Infrastructure.Persistence.Triggers;

public class DriverChangeTrigger(ICacheService cacheService) : IAfterSaveTrigger<Driver>
{
    public async Task AfterSave(ITriggerContext<Driver> context, CancellationToken cancellationToken)
    {
        await cacheService.RemoveRecordsByPrefixAsync(CachePrefixConstants.DriversKey, cancellationToken);
    }
}