using BSMS.Application.Contracts.Persistence;
using MapsterMapper;
using MediatR;

namespace BSMS.Application.Features.Route.GetTopByRevenue;

public record GetTopRoutesByRevenueQuery() : IRequest<List<GetTopRoutesByRevenueResponse>>;

public record GetTopRoutesByRevenueResponse(
    string RouteName,
    decimal TotalRevenue
);
public class GetTopRoutesByRevenueQueryHandler(
    IRouteRepository repository,
    IMapper mapper)
        : IRequestHandler<GetTopRoutesByRevenueQuery, List<GetTopRoutesByRevenueResponse>>
{
    public async Task<List<GetTopRoutesByRevenueResponse>> Handle(GetTopRoutesByRevenueQuery request, CancellationToken cancellationToken)
    {
        return mapper.Map<List<GetTopRoutesByRevenueResponse>>(await repository.GetTopRouteRevenuesAsync(10));
    }
}