using BSMS.Application.Contracts.Caching;
using BSMS.Application.Helpers;
using BSMS.Core.Entities;
using EntityFrameworkCore.Triggered;

namespace BSMS.Infrastructure.Persistence.Triggers;

public class PassengerChangeTrigger(ICacheService cacheService) : IAfterSaveTrigger<Passenger>
{
    public async Task AfterSave(ITriggerContext<Passenger> context, CancellationToken cancellationToken)
    {
        await cacheService.RemoveRecordsByPrefixAsync(CachePrefixConstants.PassengersKey, cancellationToken);
    }
}