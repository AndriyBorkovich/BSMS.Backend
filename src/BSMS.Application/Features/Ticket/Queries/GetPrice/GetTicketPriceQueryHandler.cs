using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Helpers;
using MediatR;

namespace BSMS.Application.Features.Ticket.Queries.GetPrice;

public record GetTicketPriceQuery(
    int StartStopId,
    int EndStopId
) : IRequest<MethodResult<GetTicketPriceResponse>>;

public record GetTicketPriceResponse(
    decimal Price
);

public class GetTicketPriceQueryHandler(
    IStopRepository repository,
    MethodResultFactory methodResultFactory
) : IRequestHandler<GetTicketPriceQuery, MethodResult<GetTicketPriceResponse>>
{
    public async Task<MethodResult<GetTicketPriceResponse>> Handle(GetTicketPriceQuery request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<GetTicketPriceResponse>();

        var startStop = await repository.GetByIdAsync(request.StartStopId);
        var endStop = await repository.GetByIdAsync(request.EndStopId);
        if (startStop == null)
        {
            result.SetError("Start stop not found", System.Net.HttpStatusCode.NotFound);
            return result;
        }

        if (endStop == null)
        {
            result.SetError("End stop not found", System.Net.HttpStatusCode.NotFound);
            return result;
        }

        if (startStop.RouteId != endStop.RouteId)
        {
            result.SetError("Stops belong to different routes", System.Net.HttpStatusCode.BadRequest);
            return result;
        }

        // Calculate the price based on the distances
        int distance1 = startStop?.DistanceToPrevious ?? 0;
        int distance2 = endStop?.DistanceToPrevious ?? 1; // If endStop is null, set default distance to 1
        decimal price = Math.Abs(distance2 - distance1) * 0.3m;

        result.Data = new GetTicketPriceResponse(price);
        
        return result;
    }
}