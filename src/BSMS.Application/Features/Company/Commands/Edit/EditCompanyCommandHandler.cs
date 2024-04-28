using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Extensions;
using BSMS.Application.Features.Common;
using BSMS.Application.Helpers;
using FluentValidation;
using MapsterMapper;
using MediatR;

namespace BSMS.Application.Features.Company.Commands.Edit;

public record EditCompanyCommand(
    int CompanyId,
    string Name,
    string Phone,
    string Email,
    string Street,
    string City,
    string Country,
    string ZipCode) : IRequest<MethodResult<MessageResponse>>;

public class EditCompanyCommandHandler(
    ICompanyRepository repository,
    IMapper mapper,
    IValidator<EditCompanyCommand> validator,
    MethodResultFactory methodResultFactory)
        : IRequestHandler<EditCompanyCommand, MethodResult<MessageResponse>>
{
    public async Task<MethodResult<MessageResponse>> Handle(EditCompanyCommand request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<MessageResponse>();

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            result.SetError(validationResult.Errors.ToResponse(), System.Net.HttpStatusCode.BadRequest);
            return result;
        }

        var company = await repository.GetByIdAsync(request.CompanyId);

        mapper.Map<EditCompanyCommand, Core.Entities.Company>(request, company);

        await repository.UpdateAsync(company);

        result.Data = new MessageResponse($"Company {request.CompanyId} was edited");

        return result;
    }
}
