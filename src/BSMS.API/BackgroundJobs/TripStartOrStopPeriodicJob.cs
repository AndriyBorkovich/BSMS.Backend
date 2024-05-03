
using BSMS.Core.Entities;
using BSMS.Core.Enums;
using BSMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BSMS.API.BackgroundJobs;
/// <summary>
/// Periodic job that handles start or stop of trips based on current time 
/// </summary>
/// <param name="serviceProvider"></param>
/// <param name="logger"></param>
public class TripStartOrStopPeriodicJob(
    IServiceProvider serviceProvider,
    ILogger<TripStartOrStopPeriodicJob> logger) : BackgroundService
{
    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const int offset = 60;
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BusStationContext>();

                var currentTime = DateTime.Now;
                var currentHour = currentTime.Hour;
                var currentMinute = currentTime.Minute;
                var currentDay = currentTime.DayOfWeek;

                // Query for bus schedule entries where departure time has arrived
                var tripsToStart = await dbContext.Trips
                    .Where(t => t.Status != TripStatus.Canceled && t.Status != TripStatus.Delayed
                        && t.BusScheduleEntry != null
                        && t.BusScheduleEntry.Day == currentDay
                        && t.BusScheduleEntry.DepartureTime.Minute == currentMinute
                        && t.BusScheduleEntry.DepartureTime.Hour == currentHour)
                    .ToListAsync(cancellationToken: stoppingToken);

                var tripsToSetInTransit = new List<Trip>();
                foreach (var trip in tripsToStart)
                {
                    if (trip is not null)
                    {
                        if (await CanStart(dbContext, trip))
                        {
                            trip.Status = TripStatus.InTransit;
                            trip.DepartureTime = DateTime.Now;
                        }
                        else
                        {
                            trip.Status = TripStatus.Delayed;
                        }

                        tripsToSetInTransit.Add(trip);
                    }
                }

                if (tripsToSetInTransit.Count is not 0)
                {
                    await dbContext.BulkUpdateAsync(tripsToSetInTransit);
                    logger.LogInformation("Some trips has started");
                }

                // Query for bus schedule entries where arival time has come
                var updatedRows = await dbContext.Trips
                    .Where(t => t.Status == TripStatus.InTransit
                        && t.BusScheduleEntry != null
                        && t.BusScheduleEntry.Day == currentDay
                        && t.BusScheduleEntry.ArrivalTime.Minute == currentMinute
                        && t.BusScheduleEntry.ArrivalTime.Hour == currentHour)
                    .ExecuteUpdateAsync(
                        t => t.SetProperty(e => e.Status, TripStatus.Completed)
                              .SetProperty(e => e.ArrivalTime, DateTime.Now),
                        cancellationToken: stoppingToken);
                        
                if(updatedRows > 0)
                {
                    logger.LogInformation("Some trips has completed");
                }

                // Double check delayed & already scheduled trips which somehow didn't start, try to restart them

                var delayedTrips = await dbContext.Trips
                                        .Where(t => t.Status == TripStatus.Delayed 
                                            || t.Status == TripStatus.Scheduled
                                            && t.DepartureTime != null 
                                            && t.DepartureTime.Value.Date == currentTime.Date)
                                        .ToListAsync(cancellationToken: stoppingToken);

                var tripsToRestart = new List<Trip>();
                foreach (var trip in delayedTrips)
                {
                    if (await CanStart(dbContext, trip))
                    {
                        trip.Status = TripStatus.InTransit;
                        trip.DepartureTime = DateTime.Now;
                    }
                    else
                    {
                        trip.Status = TripStatus.Delayed;
                    }

                    tripsToRestart.Add(trip);
                }

                if (tripsToRestart.Count is not 0)
                {
                    await dbContext.BulkUpdateAsync(tripsToRestart);
                }

                await dbContext.SaveChangesAsync(stoppingToken);
            }

            // Delay execution to avoid continuous looping
            await Task.Delay(TimeSpan.FromSeconds(offset), stoppingToken);
        }
    }

    private async Task<bool> CanStart(BusStationContext dbContext, Trip trip)
    {
        var busCapacity = await dbContext.Buses
                            .Where(bus => bus.BusScheduleEntries.Any(b => b.BusScheduleEntryId == trip.BusScheduleEntryId))
                            .Select(bus => bus.Capacity)
                            .FirstOrDefaultAsync();

        var boughtTicketCount = await dbContext.TicketPayments
            .CountAsync(
                p => p.TripId == trip.TripId && p.Ticket.Status == TicketStatus.Active);

        return boughtTicketCount >= busCapacity / 2; // trip can start only if passenger bought at least 50% of possible tickets
    }
}
