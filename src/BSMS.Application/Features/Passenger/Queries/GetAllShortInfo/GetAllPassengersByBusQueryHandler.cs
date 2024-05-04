using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Helpers;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BSMS.Application.Features.Passenger.Queries.GetAllShortInfo;

public record GetAllPassengersShortInfoQuery(int? BusId) : IRequest<MethodResult<List<GetAllPassengersShortInfoResponse>>>;

public record GetAllPassengersShortInfoResponse(
    int PassengerId,
    string FullName
);

public class GetAllPassengersShortInfoQueryHandler(
    IPassengerRepository passengerRepository,
    IBusRepository busRepository,
    MethodResultFactory methodResultFactory) 
        : IRequestHandler<GetAllPassengersShortInfoQuery, MethodResult<List<GetAllPassengersShortInfoResponse>>>
{
    public async Task<MethodResult<List<GetAllPassengersShortInfoResponse>>> Handle(GetAllPassengersShortInfoQuery request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<List<GetAllPassengersShortInfoResponse>>();

        var query = passengerRepository.GetAll().AsNoTracking();

        // if bus ID is null - then get all passengers from DB, 
        // else get only those, who bought at least one ticket for trip for this bus
        if (request.BusId is not null)
        {
            var busExists = await busRepository.AnyAsync(b => b.BusId == request.BusId);
            if (!busExists)
            {
                result.SetError("Requested bus don't exists", System.Net.HttpStatusCode.NotFound);
                return result;
            }

            query = query.Where(p => p.Payments.Any(tp => tp.Trip.BusScheduleEntry.Bus.BusId == request.BusId));
        }

        result.Data = await query.ProjectToType<GetAllPassengersShortInfoResponse>().ToListAsync(cancellationToken);

        return result;
    }
}