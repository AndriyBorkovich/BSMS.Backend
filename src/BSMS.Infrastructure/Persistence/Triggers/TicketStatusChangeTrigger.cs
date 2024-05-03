using BSMS.Core.Entities;
using BSMS.Core.Enums;
using EntityFrameworkCore.Triggered;
using Microsoft.EntityFrameworkCore;

namespace BSMS.Infrastructure.Persistence.Triggers;
/// <summary>
/// used for correct seat availability handling
/// </summary>
/// <param name="busStationContext"></param>
public class TicketStatusChangeTrigger(BusStationContext busStationContext) : IAfterSaveTrigger<Ticket>
{
    public async Task AfterSave(ITriggerContext<Ticket> context, CancellationToken cancellationToken)
    {
        if (context.ChangeType == ChangeType.Modified)
        {
            if (context.Entity.Status is TicketStatus.InUse)
            {
                await busStationContext.Seats.Where(s => s.Tickets.Any(t => t.TicketId == context.Entity.TicketId))
                .ExecuteUpdateAsync(
                    s => s.SetProperty(s => s.IsFree, false), cancellationToken: cancellationToken);
            }
            else
            {
                await busStationContext.Seats.Where(s => s.Tickets.Any(t => t.TicketId == context.Entity.TicketId))
                .ExecuteUpdateAsync(
                    s => s.SetProperty(s => s.IsFree, true), cancellationToken: cancellationToken);
            }
        }
    }
}