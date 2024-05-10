using BSMS.Application.Features.Common;
using BSMS.Core.Entities;

namespace BSMS.Application.Contracts.Persistence;

public interface IRouteRepository : IGenericRepository<Route>
{
    Task<List<RouteRating>> GetRouteRatings(int topCount);
    Task<List<RouteRevenue>> GetTopRouteRevenuesAsync(int topCount);
}