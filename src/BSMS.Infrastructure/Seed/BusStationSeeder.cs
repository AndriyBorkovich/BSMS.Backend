using Bogus;
using BSMS.Core.Entities;
using BSMS.Core.Enums;
using BSMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BSMS.Infrastructure.Seed;

public class BusStationSeeder(IServiceProvider serviceProvider) : IBusStationSeeder
{
    public void GenerateScheduleForBuses(int busId, int entriesForEachDay = 2)
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

        context.BusScheduleEntries.BulkInsert(bogus.Generate(), opt => {
            opt.BatchSize = 1000;
            opt.InsertIfNotExists = true;
        });
        context.SaveChanges();
    }

    public void GenerateBusReviews()
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

        var busReviews = reviews.Generate(5000);

        context.BusReviews.BulkInsert(busReviews, opt => {
            opt.InsertIfNotExists = true;
            opt.BatchSize = 500;
        });

        context.SaveChanges();
    }
}