using System.Net;
using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Features.Common;
using BSMS.Application.Helpers;
using MediatR;

namespace BSMS.Application.Features.Route.Commands.Delete;

public record DeleteRouteCommand(int Id) : IRequest<MethodResult<MessageResponse>>;
    
public class DeleteRouteCommandHandler(
        IRouteRepository repository,
        MethodResultFactory methodResultFactory)
    : IRequestHandler<DeleteRouteCommand, MethodResult<MessageResponse>>
{
    public async Task<MethodResult<MessageResponse>> Handle(DeleteRouteCommand request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<MessageResponse>();

        var route = await repository.GetByIdAsync(request.Id);
        if (route is null)
        {
            result.SetError($"Route with ID {request.Id} not found", HttpStatusCode.NotFound);
            return result;
        }

        await repository.DeleteAsync(route);

        result.Data = new MessageResponse("Route was successfully deleted");
        
        return result;
    }
}