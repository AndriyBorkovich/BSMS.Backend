using System.Net;
using BSMS.Application.Contracts.Caching;
using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Extensions;
using BSMS.Application.Features.Common;
using BSMS.Application.Helpers;
using FluentValidation;
using MapsterMapper;
using MediatR;

namespace BSMS.Application.Features.Passenger.Commands.Create;

public record CreatePassengerCommand(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Email) : IRequest<MethodResult<CreatedEntityResponse>>;

public class CreatePassengerCommandHandler(
    IPassengerRepository repository,
    IValidator<CreatePassengerCommand> validator,
    IMapper mapper,
    ICacheService cacheService,
    MethodResultFactory methodResultFactory) : IRequestHandler<CreatePassengerCommand, MethodResult<CreatedEntityResponse>>
{
    public async Task<MethodResult<CreatedEntityResponse>> Handle(
        CreatePassengerCommand request, 
        CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<CreatedEntityResponse>();
        
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            result.SetError(validationResult.Errors.ToResponse(), HttpStatusCode.BadRequest);
            return result;
        }

        var passenger = mapper.Map<Core.Entities.Passenger>(request);

        await repository.InsertAsync(passenger);

        await cacheService.RemoveRecordsByPrefixAsync(CachePrefixConstants.PassengersKey, cancellationToken);

        result.Data = new CreatedEntityResponse(passenger.PassengerId);

        return result;
    }
}