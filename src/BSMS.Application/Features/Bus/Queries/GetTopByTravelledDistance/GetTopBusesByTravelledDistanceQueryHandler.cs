using BSMS.Application.Contracts.Persistence;
using MapsterMapper;
using MediatR;

namespace BSMS.Application.Features.Bus.Queries.GetTopByTravelledDistance;

public record GetTopBusesByTravelledDistanceQuery() : IRequest<List<GetTopBusesByTravelledDistanceResponse>>;

public record GetTopBusesByTravelledDistanceResponse(
    string BusNumber,
    int DistanceTravelled
);
public class GetTopBusesByTravelledDistanceQueryHandler(
    IBusRepository repository,
    IMapper mapper)
        : IRequestHandler<GetTopBusesByTravelledDistanceQuery, List<GetTopBusesByTravelledDistanceResponse>>
{
    public async Task<List<GetTopBusesByTravelledDistanceResponse>> Handle(GetTopBusesByTravelledDistanceQuery request, CancellationToken cancellationToken)
    {
        return mapper.Map<List<GetTopBusesByTravelledDistanceResponse>>(await repository.GetMostCrossedDistanceAsync());
    }
}