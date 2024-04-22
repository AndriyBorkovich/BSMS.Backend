using BSMS.Application.Contracts.Caching;
using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Extensions;
using BSMS.Application.Features.Common;
using BSMS.Application.Helpers;
using FluentValidation;
using MapsterMapper;
using MediatR;

namespace BSMS.Application.Features.Bus.Commands.Create;

public class CreateBusCommandHandler(
        IBusRepository repository, 
        IMapper mapper,
        IValidator<CreateBusCommand> validator,
        ICacheService cacheService,
        MethodResultFactory methodResultFactory)
    : IRequestHandler<CreateBusCommand, MethodResult<CreatedEntityResponse>>
{
    public async Task<MethodResult<CreatedEntityResponse>> Handle(CreateBusCommand request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<CreatedEntityResponse>();
        
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if(!validationResult.IsValid)
        {
            result.SetError(validationResult.Errors.ToResponse(), System.Net.HttpStatusCode.BadRequest);
            return result;
        }
        
        var bus = mapper.Map<Core.Entities.Bus>(request);

        await repository.InsertAsync(bus);

        await cacheService.RemoveRecordsByPrefixAsync(CachePrefixConstants.BusesKey, cancellationToken);
        
        result.Data = new CreatedEntityResponse(bus.BusId);
        
        return result;
    }
}