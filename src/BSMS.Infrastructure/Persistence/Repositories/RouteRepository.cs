using BSMS.Application.Contracts;
using BSMS.Core.Entities;

namespace BSMS.Infrastructure.Persistence.Repositories;

public class RouteRepository : GenericRepository<Route>, IRouteRepository
{
    public RouteRepository(BusStationContext context) : base(context)
    {
    }
}