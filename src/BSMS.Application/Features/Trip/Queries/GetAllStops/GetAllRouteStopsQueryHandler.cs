using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Helpers;
using BSMS.Core.Entities;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BSMS.Application.Features.Trip.Queries.GetAllStops;

public record GetAllRouteStopsQuery(int TripId) : IRequest<MethodResult<List<GetAllRouteStopsResponse>>>;

public record GetAllRouteStopsResponse(
    int StopId,
    string Name
);

public class GetAllRouteStopsQueryHandler(
    ITripRepository tripRepository,
    IMapper mapper,
    MethodResultFactory methodResultFactory
) : IRequestHandler<GetAllRouteStopsQuery, MethodResult<List<GetAllRouteStopsResponse>>>
{
    public async Task<MethodResult<List<GetAllRouteStopsResponse>>> Handle(GetAllRouteStopsQuery request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<List<GetAllRouteStopsResponse>>();

        var tripExists = await tripRepository.AnyAsync(t => t.TripId == request.TripId);
        if (!tripExists)
        {
            result.SetError("Trip not found", System.Net.HttpStatusCode.NotFound);
            return result;
        }

        var orderedStops = await tripRepository.GetAll()
                                .Where(t => t.TripId == request.TripId)
                                .SelectMany(t => t.BusScheduleEntry.Route.Stops)
                                .OrderBy(s => s.DistanceToPrevious)
                                .Select(s => new Stop { StopId = s.StopId, Name = s.Name })
                                .ToListAsync(cancellationToken);

        result.Data = mapper.Map<List<GetAllRouteStopsResponse>>(orderedStops);

        return result;
    }
}