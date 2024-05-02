using BSMS.Core.Entities;

namespace BSMS.Infrastructure.Seed;

public interface IBusStationSeeder
{
    void GenerateScheduleForBuses(int busId, int numberOfEntries);
    void GenerateBusReviews();
}