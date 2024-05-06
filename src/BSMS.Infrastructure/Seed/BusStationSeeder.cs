using System.Collections.Concurrent;
using Bogus;
using BSMS.Core.Entities;
using BSMS.Core.Enums;
using BSMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BSMS.Infrastructure.Seed;

public class BusStationSeeder(
    IServiceProvider serviceProvider,
    ILogger<BusStationSeeder> logger) : IBusStationSeeder
{
    public async Task GenerateScheduleForBuses(int busId, int entriesForEachDay = 2)
    {
        var gRandom = new Randomizer();
        var random = new Random();
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BusStationContext>();

        var routes = context.Routes.Select(r => r.RouteId).ToList();
        var lastEntryId = context.BusScheduleEntries?.Select(e => e.BusScheduleEntryId)?.ToList().LastOrDefault() ?? 0;
        //context.Database.ExecuteSqlInterpolated($"DBCC CHECKIDENT('BusScheduleEntries', RESEED, {lastEntryId});");
        var bogus = new Faker<List<BusScheduleEntry>>()
        .CustomInstantiator(f =>
        {
            var entries = new List<BusScheduleEntry>();
            var randomRouteId = f.PickRandom(routes);

            // Define the range of possible times (in hours)
            int minHours = 1; // Minimum hours
            int maxHours = 4; // Maximum hours

            // Generate a random number of hours within the range
            int randomHours = random.Next(minHours, maxHours + 1);
            var scheduledHoursPerDay = new Dictionary<DayOfWeek, HashSet<(TimeOnly DepartureTime, TimeOnly ArrivalTime)>>();

            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                var scheduledHours = new HashSet<(TimeOnly DepartureTime, TimeOnly ArrivalTime)>();
                for (int i = 1; i <= entriesForEachDay; i++)
                {
                    var existingHours = scheduledHoursPerDay.GetValueOrDefault(day, new HashSet<(TimeOnly, TimeOnly)>());

                    // Define the desired route time
                    var desiredRouteTime = TimeSpan.FromHours(randomHours); // Example: 2 hours

                    // Generate a random departure time
                    var departureTime = f.Date.BetweenTimeOnly(TimeOnly.MinValue, TimeOnly.MaxValue.AddHours(-desiredRouteTime.Hours));

                    // Calculate the arrival time based on the departure time and desired route time
                    var arrivalTime = departureTime.Add(desiredRouteTime);

                    // Adjust departure and arrival times to avoid intersections
                    while (existingHours.Any(h => !(arrivalTime < h.Item1 || departureTime > h.Item2)))
                    {
                        departureTime = f.Date.BetweenTimeOnly(TimeOnly.MinValue, TimeOnly.MaxValue.AddHours(-desiredRouteTime.Hours));
                        arrivalTime = f.Date.BetweenTimeOnly(departureTime, TimeOnly.MaxValue);
                    }

                    // Add the scheduled hours to the set
                    scheduledHours.Add((departureTime, arrivalTime));
                    scheduledHoursPerDay[day] = scheduledHours;
                    var entry = new BusScheduleEntry
                    {
                        BusId = busId,
                        RouteId = randomRouteId,
                        DepartureTime = departureTime,
                        ArrivalTime = arrivalTime,
                        MoveDirection = i % 2 != 0 ? Direction.ToDestination : Direction.FromDestination,
                        Day = day
                    };

                    entries.Add(entry);
                }
            }

            return entries;
        });

        await context.BusScheduleEntries.BulkInsertOptimizedAsync(bogus.Generate(), opt =>
        {
            opt.BatchSize = 1000;
            opt.InsertIfNotExists = true;
        });
    }

    public async Task GenerateBusReviews()
    {
        var random = new Randomizer();
        var faker = new Faker();

        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BusStationContext>();

        var busIds = context.Buses.Select(b => b.BusId).ToList();
        var passengerIds = context.Passengers.Select(p => p.PassengerId).ToList();

        var reviews = new Faker<BusReview>()
            .RuleFor(r => r.BusId, f => f.PickRandom(busIds))
            .RuleFor(r => r.PassengerId, f => f.PickRandom(passengerIds))
            .RuleFor(r => r.ComfortRating, f => f.Random.Number(1, 5))
            .RuleFor(r => r.PunctualityRating, f => f.Random.Number(1, 5))
            .RuleFor(r => r.PriceQualityRatioRating, f => f.Random.Number(1, 5))
            .RuleFor(r => r.InternetConnectionRating, f => f.Random.Number(1, 5))
            .RuleFor(r => r.SanitaryConditionsRating, f => f.Random.Number(1, 5))
            .RuleFor(r => r.Comments, f => f.Random.Words(f.Random.Number(1, 10)))
            .RuleFor(r => r.ReviewDate, f => f.Date.Recent());

        var busReviews = reviews.Generate(10000);

        await context.BusReviews.BulkInsertOptimizedAsync(busReviews, opt =>
        {
            opt.InsertIfNotExists = true;
            opt.BatchSize = 500;
        });

        context.SaveChanges();
    }

    public async Task GenerateTicketsAndPaymentsForTrip(int tripId)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BusStationContext>();

        var tripScheduleEntries = dbContext.BusScheduleEntries.Where(be => be.Trips.Any(t => t.TripId == tripId));

        var seatIds = await tripScheduleEntries
                           .Select(e => e.Bus)
                           .SelectMany(b => b.Seats)
                           .Where(s => s.IsFree)
                           .Select(s => s.SeatId)
                           .ToListAsync();

        if (seatIds.Count is not default(int))
        {
            var routeStops = tripScheduleEntries.Select(be => be.Route).SelectMany(r => r.Stops);

            var startStop = await routeStops.FirstAsync();

            var ticketsToGenerate = new Random().Next(1, seatIds.Count + 1);

            var passengersIds = await dbContext.Passengers.Select(p => p.PassengerId).ToListAsync();

            var tripDepartureTime = (await dbContext.Trips.FirstAsync(t => t.TripId == tripId)).DepartureTime;

            var availableSeatIds = new HashSet<int>(seatIds);

            var f = new Faker();

            var tickets = new List<Ticket>();

            for (int i = 0; i < ticketsToGenerate; i++)
            {
                var distance = startStop.DistanceToPrevious ?? 0; // zero distance for starting stop on route
                var distancesToStart = await routeStops
                                    .Where(s => s.StopId != startStop.StopId && s.DistanceToPrevious >= distance)
                                    .ToDictionaryAsync(s => s.StopId, s => s.DistanceToPrevious);

                // ordered stops based on their distance to the start stop
                var orderedStopsIds = distancesToStart.OrderBy(kv => kv.Value).Select(kv => kv.Key);

                var endStopId = f.PickRandom(orderedStopsIds);

                // Pick a random seat that hasn't been choosen yet and remove the choosen seat from available seats
                var choosenSeatId = f.PickRandom(availableSeatIds.ToList());
                availableSeatIds.Remove(choosenSeatId);

                var newTicket = new Ticket
                {
                    SeatId = f.PickRandom(seatIds),
                    StartStopId = startStop.StopId,
                    EndStopId = endStopId,
                    Status = TicketStatus.InUse, // seat availability will be determined by this value in SQL trigger
                    IsSold = true,
                    Payment = new TicketPayment
                    {
                        PassengerId = f.PickRandom(passengersIds),
                        TripId = tripId,
                        PaymentType = f.PickRandom<PaymentType>(),
                        PaymentDate = f.Date.Between(
                            tripDepartureTime!.Value.AddHours(new Random().Next(-3, 0)),
                            tripDepartureTime!.Value)
                    }
                };

                tickets.Add(newTicket);
            }

            await dbContext.BulkInsertOptimizedAsync(tickets, opt => opt.IncludeGraph = true);

            logger.LogInformation("Passengers have bought {0} tickets", ticketsToGenerate);
        }
        else
        {
            logger.LogInformation("Trip have no free tickets to buy");
        }
    }
}