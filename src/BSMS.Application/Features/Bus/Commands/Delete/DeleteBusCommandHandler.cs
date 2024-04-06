using System.Net;
using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Features.Common;
using BSMS.Application.Helpers;
using MediatR;

namespace BSMS.Application.Features.Bus.Commands.Delete;

public record DeleteBusCommand(int Id) : IRequest<MethodResult<MessageResponse>>;

public class DeleteBusCommandHandler(
    IBusRepository repository,
    MethodResultFactory methodResultFactory) 
        : IRequestHandler<DeleteBusCommand, MethodResult<MessageResponse>>
{
    public async Task<MethodResult<MessageResponse>> Handle(DeleteBusCommand request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<MessageResponse>();

        var bus = await repository.GetByIdAsync(request.Id);
        if (bus is null)
        {
            result.SetError($"Bus with ID {request.Id} not found", HttpStatusCode.NotFound);
            return result;
        }

        await repository.DeleteAsync(bus);
        
        result.Data = new MessageResponse("Bus was deleted successfully");
        
        return result;
    }
}