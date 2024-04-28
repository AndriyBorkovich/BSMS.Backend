using BSMS.Application.Contracts.Caching;
using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Extensions;
using BSMS.Application.Features.Common;
using BSMS.Application.Helpers;
using FluentValidation;
using MapsterMapper;
using MediatR;

namespace BSMS.Application.Features.Driver.Commands.Edit;

public record EditDriverCommand (
    int DriverId,
    string FirstName,
    string LastName,
    string DriverLicense,
    int CompanyId) : IRequest<MethodResult<MessageResponse>>;

public class EditDriverCommandHandler(
    IDriverRepository repository,
    IMapper mapper,
    IValidator<EditDriverCommand> validator,
    ICacheService cacheService,
    MethodResultFactory methodResultFactory
) : IRequestHandler<EditDriverCommand, MethodResult<MessageResponse>>
{
    public async Task<MethodResult<MessageResponse>> Handle(EditDriverCommand request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<MessageResponse>();

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            result.SetError(validationResult.Errors.ToResponse(), System.Net.HttpStatusCode.BadRequest);
            return result;
        }

        var driver = await repository.GetByIdAsync(request.DriverId);

        mapper.Map<EditDriverCommand, Core.Entities.Driver>(request, driver);

        await repository.UpdateAsync(driver);

        await cacheService.RemoveRecordsByPrefixAsync(CachePrefixConstants.BusesKey, cancellationToken);

        result.Data = new MessageResponse($"Driver with ID {request.DriverId} was edited successfully");

        return result;
    }
}