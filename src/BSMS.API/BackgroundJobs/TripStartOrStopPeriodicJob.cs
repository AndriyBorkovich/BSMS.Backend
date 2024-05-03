using System.Collections.Concurrent;
using Bogus;
using BSMS.Core.Entities;
using BSMS.Core.Enums;
using BSMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BSMS.API.BackgroundJobs;
/// <summary>
/// Periodic job that handles start or stop of trips based on current date 
/// </summary>
/// <param name="serviceProvider"></param>
/// <param name="logger"></param>
public class TripStartOrStopPeriodicJob(
    IServiceProvider serviceProvider,
    ILogger<TripStartOrStopPeriodicJob> logger) : BackgroundService
{
    private class TripsAvailabilityData
    {
        public int TripId { get; set; }
        public int BusCapaity { get; set; }
        public int BoughtTicketsCount { get; set; }
        public bool CanStartTrip()
        {
            return BoughtTicketsCount >= BusCapaity / 2;
        }
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const int offset = 60;
        while (!stoppingToken.IsCancellationRequested)
        {
            await HandleTripsAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(offset), stoppingToken);
        }
    }

    private async Task HandleTripsAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BusStationContext>();

        var currentDateTime = DateTime.Now;

        await HandleDepartingAsync(dbContext, currentDateTime, stoppingToken);
        await HandleArrivingAsync(dbContext, currentDateTime, stoppingToken);
        await TryRestartAsync(dbContext, currentDateTime, stoppingToken);
    }

    private async Task HandleDepartingAsync(
        BusStationContext dbContext, DateTime currentDateTime, CancellationToken stoppingToken)
    {
        var currentHour = currentDateTime.Hour;
        var currentMinute = currentDateTime.Minute;
        var currentDay = currentDateTime.DayOfWeek;

        // Query for bus schedule entries where departure time has arrived
        var tripCandidatesToStart = await dbContext.Trips
            .Where(t => t.Status != TripStatus.Canceled && t.Status != TripStatus.Delayed
                && t.BusScheduleEntry != null
                && t.BusScheduleEntry.Day == currentDay
                && t.BusScheduleEntry.DepartureTime.Minute == currentMinute
                && t.BusScheduleEntry.DepartureTime.Hour == currentHour)
            .ToListAsync(cancellationToken: stoppingToken);

        var seatAvailabilityData = await GetTripsReadyToTransitDataAsync(dbContext, currentDateTime, stoppingToken);

        var tripsToSetInTransit = new List<Trip>();
        foreach (var trip in tripCandidatesToStart)
        {
            if (trip is not null)
            {
                var availabilityEntry = seatAvailabilityData.Find(d => d.TripId == trip.TripId);
                if (availabilityEntry != null && availabilityEntry.CanStartTrip())
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
            logger.LogInformation("{0} trips has started", tripsToSetInTransit.Count);
        }
    }

    private async Task HandleArrivingAsync(
       BusStationContext dbContext, DateTime currentDateTime, CancellationToken stoppingToken)
    {
        var currentHour = currentDateTime.Hour;
        var currentMinute = currentDateTime.Minute;
        var currentDay = currentDateTime.DayOfWeek;

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
                stoppingToken);

        if (updatedRows > 0)
        {
            logger.LogInformation("{0} trips has completed", updatedRows);
        }
    }

    private async Task TryRestartAsync(BusStationContext dbContext, DateTime currentTime, CancellationToken stoppingToken)
    {
        // Double check delayed & already scheduled trips which somehow didn't start, try to restart them
        var delayedTrips = await dbContext.Trips
                                .Where(t => t.Status == TripStatus.Delayed
                                    || t.Status == TripStatus.Scheduled
                                    && t.DepartureTime != null
                                    && t.DepartureTime.Value.Date == currentTime.Date)
                                .ToListAsync(stoppingToken);

        var seatAvailabilityData = await GetTripsReadyToTransitDataAsync(dbContext, currentTime, stoppingToken);

        var tripsToRestart = new List<Trip>();
        foreach (var trip in delayedTrips)
        {
            if (trip is not null)
            {
                var availabilityEntry = seatAvailabilityData.Find(d => d.TripId == trip.TripId);
                if (availabilityEntry != null && availabilityEntry.CanStartTrip())
                {
                    trip.Status = TripStatus.InTransit;
                    // restart with new 
                    var diff = trip.ArrivalTime - trip.DepartureTime;
                    trip.DepartureTime = DateTime.Now;
                    trip.ArrivalTime = trip.DepartureTime + diff;
                }
                else
                {
                    trip.Status = TripStatus.Delayed;
                }

                tripsToRestart.Add(trip);
            }
        }

        if (tripsToRestart.Count is not 0)
        {
            await dbContext.BulkUpdateAsync(tripsToRestart, stoppingToken);
        }
    }

    private async Task<List<TripsAvailabilityData>> GetTripsReadyToTransitDataAsync(
        BusStationContext dbContext, DateTime todayDateTime, CancellationToken cancellationToken)
    {
        return await dbContext.Trips.AsNoTracking()
            .Where(t => t.DepartureTime != null && t.DepartureTime.Value.Date == todayDateTime.Date)
            .Select(t => new TripsAvailabilityData
            {
                TripId = t.TripId,
                BusCapaity = t.BusScheduleEntry.Bus.Capacity,
                BoughtTicketsCount = t.BoughtTickets.Count()
            })
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
