
using BSMS.Infrastructure.Persistence;
using BSMS.Infrastructure.Seed;
using LinqKit;

namespace BSMS.API.BackgroundJobs;
/// <summary>
/// Seed database with randomly generated data
/// </summary>
public class DatabaseSeedJob(
    IServiceScopeFactory scopeFactory
) : IHostedService
{
    /// <inheritdoc/>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IBusStationSeeder>();
        var context = scope.ServiceProvider.GetRequiredService<BusStationContext>();

        // seed bus schedules
        //var busIds = context.Buses.Select(b => b.BusId).ToList();
        //busIds.ForEach(id => seeder.GenerateScheduleForBuses(id, new Random().Next(2, 5)));

        //seeder.GenerateBusReviews();
    }

    /// <inheritdoc/>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
