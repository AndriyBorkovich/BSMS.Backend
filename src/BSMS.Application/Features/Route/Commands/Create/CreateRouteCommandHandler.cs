using System.Net;
using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Extensions;
using BSMS.Application.Features.Common;
using BSMS.Application.Helpers;
using BSMS.Core.Entities;
using FluentValidation;
using MapsterMapper;
using MediatR;

namespace BSMS.Application.Features.Route.Commands.Create;

public class CreateRouteCommandHandler(
    IRouteRepository routeRepository,
    IStopRepository stopRepository,
    IMapper mapper,
    IValidator<CreateRouteCommand> validator,
    MethodResultFactory methodResultFactory) 
    : IRequestHandler<CreateRouteCommand, MethodResult<CreatedEntityResponse>>
{
    public async Task<MethodResult<CreatedEntityResponse>> Handle(CreateRouteCommand request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<CreatedEntityResponse>();

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            result.SetError(validationResult.Errors.ToResponse(), HttpStatusCode.BadRequest);
            return result;
        }
        
        // insert separately to avoid concurrency exception caused by SQL trigger
        var route = mapper.Map<Core.Entities.Route>(request);
        await routeRepository.InsertAsync(route);
        
        var stops = mapper.Map<List<Stop>>(request.StopsList);
        stops.ForEach(s => stopRepository.ExecuteCustomizedInsert(route.RouteId, s.Name, s.DistanceToPrevious.Value));

        result.Data = new CreatedEntityResponse(route.RouteId);

        return result;
    }
}