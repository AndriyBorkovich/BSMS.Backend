using System.Net;
using BSMS.Application.Contracts;
using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Extensions;
using BSMS.Application.Features.Common;
using BSMS.Application.Helpers;
using FluentValidation;
using MapsterMapper;
using MediatR;

namespace BSMS.Application.Features.Route.Commands.Create;

public class CreateRouteCommandHandler(
    IRouteRepository repository,
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
        
        var route = mapper.Map<Core.Entities.Route>(request);

        await repository.InsertAsync(route);

        result.Data = new CreatedEntityResponse(route.RouteId);

        return result;
    }
}