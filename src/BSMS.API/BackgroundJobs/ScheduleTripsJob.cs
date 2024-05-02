
using BSMS.Core.Entities;
using BSMS.Core.Enums;
using BSMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BSMS.API.BackgroundJobs;
/// <summary>
/// Schedule new trips based on current date
/// </summary>
/// <param name="serviceProvider"></param>
public class ScheduleTripsJob(
    IServiceProvider serviceProvider,
    ILogger<ScheduleTripsJob> logger) : IHostedService
{
    /// <inheritdoc/>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await GenerateTrips();
    }

    /// <inheritdoc/>

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task GenerateTrips()
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<BusStationContext>();
        var currentDateTime = DateTime.Now;
        var currentDayOfWeek = currentDateTime.DayOfWeek;

        var isTripsScheduled = await dbContext.Trips
                                        .AnyAsync(t => t.DepartureTime != null && t.DepartureTime.Value.Date == currentDateTime.Date);
                                        
        if (!isTripsScheduled)
        {
            // Query for bus schedule entries for the current day of the week
            var busSchedules = await dbContext.BusScheduleEntries
                .Where(entry => entry.Day == currentDayOfWeek)
                .ToListAsync();

            var scheduledTrips = new List<Trip>();

            foreach (var schedule in busSchedules)
            {
                scheduledTrips.Add(
                    new Trip
                    {
                        BusScheduleEntryId = schedule.BusScheduleEntryId,
                        // departure and arrival time need to be changed in future
                        DepartureTime = new DateTime(DateOnly.FromDateTime(currentDateTime), schedule.DepartureTime),
                        ArrivalTime = new DateTime(DateOnly.FromDateTime(currentDateTime), schedule.ArrivalTime),
                        Status = TripStatus.Scheduled
                    }
                );
            }

            await dbContext.BulkInsertAsync(scheduledTrips, options =>
            {
                options.BatchSize = 1000;
                options.InsertIfNotExists = true;
            });

            logger.LogInformation("Trips for {0} was scheduled successfully", currentDayOfWeek.ToString());
        }
        else
        {
            logger.LogInformation("Trips for {0} are already scheduled", currentDayOfWeek.ToString());
        }
    }
}
