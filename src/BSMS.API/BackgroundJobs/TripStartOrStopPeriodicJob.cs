
using BSMS.Core.Entities;
using BSMS.Core.Enums;
using BSMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BSMS.API.BackgroundJobs;
/// <summary>
/// Periodic job that handles start or stop of trips based on current time 
/// </summary>
/// <param name="serviceProvider"></param>
public class TripStartOrStopPeriodicJob(
    IServiceProvider serviceProvider) : BackgroundService
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
                    .Where(t => t.Status != TripStatus.Canceled
                        && t.BusScheduleEntry != null
                        && t.BusScheduleEntry.Day == currentDay
                        && t.BusScheduleEntry.DepartureTime.Minute == currentMinute
                        && t.BusScheduleEntry.DepartureTime.Hour == currentHour)
                    .ToListAsync(cancellationToken: stoppingToken);

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

                        dbContext.Trips.Update(trip);
                    }
                }

                // Query for bus schedule entries where arival time has come
                await dbContext.Trips
                    .Where(t => t.Status == TripStatus.InTransit
                        && t.BusScheduleEntry != null
                        && t.BusScheduleEntry.Day == currentDay
                        && t.BusScheduleEntry.ArrivalTime.Minute == currentMinute
                        && t.BusScheduleEntry.ArrivalTime.Hour == currentHour)
                    .ExecuteUpdateAsync(
                        t => t.SetProperty(e => e.Status, TripStatus.Completed)
                              .SetProperty(e => e.ArrivalTime, DateTime.Now),
                        cancellationToken: stoppingToken);

                // Double check delayed trips, try to start them

                var delayedTrips = await dbContext.Trips
                                        .Where(t => t.Status == TripStatus.Delayed
                                        && t.DepartureTime.Value.Date == currentTime.Date)
                                        .ToListAsync(cancellationToken: stoppingToken);

                foreach (var trip in delayedTrips)
                {
                    if (await CanStart(dbContext, trip))
                    {
                        // Update trip status to "Scheduled" and set departure time to current time
                        trip.Status = TripStatus.Scheduled;
                        trip.DepartureTime = currentTime;
                    }
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
