using BSMS.Core.Entities;
using BSMS.Core.Enums;
using BSMS.Infrastructure.Persistence;
using EntityFrameworkCore.Triggered;
using Microsoft.EntityFrameworkCore;

namespace BSMS.Infrastructure;

public class TripEventsTrigger(BusStationContext busStationContext) : IAfterSaveTrigger<Trip>
{
    public async Task AfterSave(ITriggerContext<Trip> context, CancellationToken cancellationToken)
    {
        if (context.ChangeType is ChangeType.Modified)
        {
            if (context.Entity.Status is TripStatus.InTransit)
            {
                await busStationContext.Tickets
                    .Where(t => t.Payment.TripId == context.Entity.TripId)
                    .ExecuteUpdateAsync(
                        s => s.SetProperty(t => t.Status, TicketStatus.InUse), 
                        cancellationToken: cancellationToken);
            }

            if (context.Entity.Status == TripStatus.Completed)
            {
                await busStationContext.Tickets
                    .Where(t => t.Payment.TripId == context.Entity.TripId)
                    .ExecuteUpdateAsync(
                        s => s.SetProperty(t => t.Status, TicketStatus.Used), 
                        cancellationToken: cancellationToken);
            }

            if (context.Entity.Status == TripStatus.Canceled)
            {
                await busStationContext.Tickets
                    .Where(t => t.Payment.TripId == context.Entity.TripId)
                    .ExecuteUpdateAsync(
                        s => s.SetProperty(t => t.Status, TicketStatus.Cancelled), 
                        cancellationToken: cancellationToken);
            }

            await busStationContext.SaveChangesAsync(cancellationToken);
        }
    }
}
