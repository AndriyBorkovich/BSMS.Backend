using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Helpers;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BSMS.Application.Features.Trip.Queries.GetFreeSeats;

public record GetFreeSeatsForTripQuery(int TripId) : IRequest<MethodResult<List<GetFreeSeatsForTripResponse>>>;

public record GetFreeSeatsForTripResponse(
    int SeatId,
    int SeatNumber
);
public class GetFreeSeatsForTripQueryHandler(
    ITripRepository repository,
    MethodResultFactory methodResultFactory)
        : IRequestHandler<GetFreeSeatsForTripQuery, MethodResult<List<GetFreeSeatsForTripResponse>>>
{

    public async Task<MethodResult<List<GetFreeSeatsForTripResponse>>> Handle(GetFreeSeatsForTripQuery request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<List<GetFreeSeatsForTripResponse>>();

        var tripExists = await repository.AnyAsync(t => t.TripId == request.TripId);
        if (!tripExists)
        {
            result.SetError("Trip not found", System.Net.HttpStatusCode.NotFound);
            return result;
        }

        result.Data = await repository.GetAll()
                                .AsNoTracking()
                                .Where(t => t.TripId == request.TripId)
                                .Select(t => t.BusScheduleEntry.Bus)
                                .SelectMany(b => b.Seats)
                                .Where(s => s.IsFree)
                                .ProjectToType<GetFreeSeatsForTripResponse>()
                                .ToListAsync(cancellationToken);

        return result;
    }
}