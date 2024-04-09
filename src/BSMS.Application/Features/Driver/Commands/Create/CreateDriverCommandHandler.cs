using System.Net;
using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Extensions;
using BSMS.Application.Features.Common;
using BSMS.Application.Helpers;
using FluentValidation;
using MapsterMapper;
using MediatR;

namespace BSMS.Application.Features.Driver.Commands.Create;

public record CreateDriverCommand(
    string FirstName,
    string LastName,
    string DriverLicense,
    int CompanyId) : IRequest<MethodResult<CreatedEntityResponse>>;

public class CreateDriverCommandHandler(
    IDriverRepository repository,
    IMapper mapper,
    IValidator<CreateDriverCommand> validator,
    MethodResultFactory methodResultFactory) : IRequestHandler<CreateDriverCommand, MethodResult<CreatedEntityResponse>>
{
    public async Task<MethodResult<CreatedEntityResponse>> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<CreatedEntityResponse>();

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            result.SetError(validationResult.Errors.ToResponse(), HttpStatusCode.BadRequest);
            return result;
        }

        var driver = mapper.Map<Core.Entities.Driver>(request);

        await repository.InsertAsync(driver);

        result.Data = new CreatedEntityResponse(driver.DriverId);
        
        return result;
    }
}