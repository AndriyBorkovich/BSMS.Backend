using BSMS.Application.Contracts.Caching;
using BSMS.Application.Helpers;
using BSMS.Core.Entities;
using EntityFrameworkCore.Triggered;

namespace BSMS.Infrastructure.Persistence.Triggers;

public class CompanyChangeTrigger(ICacheService cacheService) : IAfterSaveTrigger<Company>
{
    public async Task AfterSave(ITriggerContext<Company> context, CancellationToken cancellationToken)
    {
        await cacheService.RemoveRecordsByPrefixAsync(CachePrefixConstants.CompaniesKey, cancellationToken);
    }
}