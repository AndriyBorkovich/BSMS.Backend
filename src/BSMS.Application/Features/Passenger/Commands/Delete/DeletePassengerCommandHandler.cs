using System.Net;
using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Features.Common;
using BSMS.Application.Helpers;
using MediatR;

namespace BSMS.Application.Features.Passenger.Commands.Delete;

public record DeletePassengerCommand(int Id) : IRequest<MethodResult<MessageResponse>>;

public class DeletePassengerCommandHandler(
        IPassengerRepository repository,
        MethodResultFactory methodResultFactory)
    : IRequestHandler<DeletePassengerCommand, MethodResult<MessageResponse>>
{
    public async Task<MethodResult<MessageResponse>> Handle(DeletePassengerCommand request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<MessageResponse>();

        var passenger = await repository.GetByIdAsync(request.Id);
        if (passenger is null)
        {
            result.SetError($"Passenger with ID {request.Id} not found", HttpStatusCode.NotFound);
            return result;
        }

        await repository.DeleteAsync(passenger);

        result.Data = new MessageResponse("Passenger was successfully deleted");
        
        return result;
    }
}