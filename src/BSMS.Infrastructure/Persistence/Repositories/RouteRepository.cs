using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Features.Common;
using BSMS.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BSMS.Infrastructure.Persistence.Repositories;

public class RouteRepository : GenericRepository<Route>, IRouteRepository
{
    public RouteRepository(BusStationContext context) : base(context)
    {
    }

    public async Task<List<RouteRating>> GetRouteRatings(int topCount)
    {
        var query = from r in Context.Routes
                    join bse in Context.BusScheduleEntries on r.RouteId equals bse.RouteId
                    join b in Context.BusesDetailsView on bse.BusId equals b.BusId
                    where b.Rating > 0.0
                    group b by new { r.RouteId, r.Origin, r.Destination } into g
                    orderby g.Average(b => b.Rating) descending
                    select new RouteRating
                    {
                        RouteName = g.Key.Origin + " - " + g.Key.Destination,
                        AverageBusRating = Math.Round(g.Average(b => b.Rating), 2)
                    };

        return await query.Take(topCount).ToListAsync();
    }

    public async Task<List<RouteRevenue>> GetTopRouteRevenuesAsync(int topCount)
    {
        var query = from r in Context.Routes
                    join bse in Context.BusScheduleEntries on r.RouteId equals bse.RouteId
                    join t in Context.Trips on bse.BusScheduleEntryId equals t.BusScheduleEntryId
                    join tp in Context.TicketPayments on t.TripId equals tp.TripId
                    join tk in Context.Tickets on tp.TicketId equals tk.TicketId
                    group tk by new { r.RouteId, r.Origin, r.Destination } into g
                    orderby g.Sum(tk => tk.Price) descending
                    select new RouteRevenue
                    {
                        RouteName = g.Key.Origin + " - " + g.Key.Destination,
                        TotalRevenue = g.Sum(tk => tk.Price)
                    };

        return await query.Take(topCount).ToListAsync();
    }
}