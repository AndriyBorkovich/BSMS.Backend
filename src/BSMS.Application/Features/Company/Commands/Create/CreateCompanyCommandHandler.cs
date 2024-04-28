using System.Net;
using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Extensions;
using BSMS.Application.Features.Common;
using BSMS.Application.Helpers;
using FluentValidation;
using MapsterMapper;
using MediatR;

namespace BSMS.Application.Features.Company.Commands.Create;

public record CreateCompanyCommand(
    string Name,
    string Phone,
    string Email,
    string Street,
    string City,
    string Country,
    string ZipCode) : IRequest<MethodResult<CreatedEntityResponse>>;

public class CreateCompanyCommandHandler(
    ICompanyRepository repository,
    IMapper mapper,
    IValidator<CreateCompanyCommand> validator,
    MethodResultFactory methodResultFactory)
        : IRequestHandler<CreateCompanyCommand, MethodResult<CreatedEntityResponse>>
{
    public async Task<MethodResult<CreatedEntityResponse>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<CreatedEntityResponse>();

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            result.SetError(validationResult.Errors.ToResponse(), HttpStatusCode.BadRequest);
            return result;
        }

        var company = mapper.Map<Core.Entities.Company>(request);

        await repository.InsertAsync(company);

        result.Data = new CreatedEntityResponse(company.CompanyId);

        return result;
    }
}