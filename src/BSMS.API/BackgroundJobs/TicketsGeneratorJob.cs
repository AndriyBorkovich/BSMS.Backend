using BSMS.Core.Enums;
using BSMS.Infrastructure.Persistence;
using BSMS.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;

namespace BSMS.API.BackgroundJobs;
/// <summary>
/// background job which simulates process of passengers' payments for ticket on specific trip
/// </summary>
/// <param name="serviceProvider"></param>
/// <param name="seeder"></param>
public class TicketGenerationJob(
    IServiceProvider serviceProvider,
    IBusStationSeeder seeder
) : BackgroundService
{
    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const int offset = 30;
        while (!stoppingToken.IsCancellationRequested)
        {
            await GenerateTickets(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(offset), stoppingToken);
        }
    }

    private async Task GenerateTickets(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BusStationContext>();

        var currentDate = DateTime.Now.Date;

        var tripsIds = await dbContext.Trips
            .Where(t => t.Status != TripStatus.Cancelled
                    && t.Status != TripStatus.Completed
                    && t.DepartureTime != null 
                    && t.DepartureTime.Value.Date == currentDate)
            .Select(t => t.TripId)
            .ToArrayAsync(cancellationToken);
        
        var tripId = Random.Shared.GetItems(tripsIds, 1).FirstOrDefault();

        if (tripId is not default(int))
        {
            await seeder.GenerateTicketsAndPaymentsForTrip(tripId);
        }
    }
}