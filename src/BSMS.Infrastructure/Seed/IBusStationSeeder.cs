using BSMS.Core.Entities;

namespace BSMS.Infrastructure.Seed;

public interface IBusStationSeeder
{
    Task GenerateScheduleForBuses(int busId, int numberOfEntries);
    Task GenerateBusReviews();
    Task GenerateTicketsAndPaymentsForTrip(int tripId);
}