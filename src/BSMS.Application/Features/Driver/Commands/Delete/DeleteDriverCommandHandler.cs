using System.Net;
using BSMS.Application.Contracts.Caching;
using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Features.Common;
using BSMS.Application.Helpers;
using MediatR;

namespace BSMS.Application.Features.Driver.Commands.Delete;

public record DeleteDriverCommand(int Id) : IRequest<MethodResult<MessageResponse>>;

public class DeleteDriverCommandHandler(
        IDriverRepository repository,
        ICacheService cacheService,
        MethodResultFactory methodResultFactory)
    : IRequestHandler<DeleteDriverCommand, MethodResult<MessageResponse>>
{
    public async Task<MethodResult<MessageResponse>> Handle(DeleteDriverCommand request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<MessageResponse>();

        var driver = await repository.GetByIdAsync(request.Id);
        if (driver is null)
        {
            result.SetError($"Driver with ID {request.Id} not found", HttpStatusCode.NotFound);
            return result;
        }

        await repository.DeleteAsync(driver);

        await cacheService.RemoveRecordsByPrefixAsync(CachePrefixConstants.DriversKey, cancellationToken);

        await cacheService.RemoveRecordsByPrefixAsync(CachePrefixConstants.BusesKey, cancellationToken);

        result.Data = new MessageResponse("Driver was successfully deleted");
        
        return result;
    }
}