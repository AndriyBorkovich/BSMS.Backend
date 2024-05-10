using BSMS.Application.Contracts.Persistence;
using MapsterMapper;
using MediatR;

namespace BSMS.Application.Features.Route.GetTopByBusRating;

public record GetTopRoutesByBusRatingQuery() : IRequest<List<GetTopRoutesByBusRatingResponse>>;

public record GetTopRoutesByBusRatingResponse(
    string RouteName,
    double AverageBusRating
);
public class GetTopRoutesByBusRatingQueryHandler(
    IRouteRepository repository,
    IMapper mapper)
        : IRequestHandler<GetTopRoutesByBusRatingQuery, List<GetTopRoutesByBusRatingResponse>>
{
    public async Task<List<GetTopRoutesByBusRatingResponse>> Handle(GetTopRoutesByBusRatingQuery request, CancellationToken cancellationToken)
    {
        return mapper.Map<List<GetTopRoutesByBusRatingResponse>>(await repository.GetRouteRatings(10));
    }
}