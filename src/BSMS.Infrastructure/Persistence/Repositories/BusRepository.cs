using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Features.Common;
using BSMS.Core.Entities;
using BSMS.Core.Views;
using Microsoft.EntityFrameworkCore;

namespace BSMS.Infrastructure.Persistence.Repositories;

public class BusRepository: GenericRepository<Bus>, IBusRepository
{
    public BusRepository(BusStationContext context) : base(context)
    {
    }

    public IQueryable<BusDetailsView> GetBusesDetails()
    {
        return Context.BusesDetailsView;
    }

    public async Task<List<BusDistance>> GetMostCrossedDistanceAsync()
    {
        var query = from b in Context.Buses
                    join bse in Context.BusScheduleEntries on b.BusId equals bse.BusId
                    join r in Context.Routes on bse.RouteId equals r.RouteId
                    join t in Context.Trips on bse.BusScheduleEntryId equals t.BusScheduleEntryId
                    where t.Status != Core.Enums.TripStatus.Cancelled
                    group r by new { b.BusId, b.Number } into g
                    orderby g.Sum(r => r.OverallDistance) descending
                    select new BusDistance
                    {
                        BusNumber = g.Key.Number,
                        DistanceTravelled = g.Sum(r => r.OverallDistance)
                    };

        return await query.Take(5).ToListAsync();
    }
}