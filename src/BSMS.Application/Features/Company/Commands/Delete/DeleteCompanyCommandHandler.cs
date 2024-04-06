using System.Net;
using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Features.Common;
using BSMS.Application.Helpers;
using MediatR;

namespace BSMS.Application.Features.Company.Commands.Delete;

public record DeleteCompanyCommand(int Id) : IRequest<MethodResult<MessageResponse>>;

public class DeleteCompanyCommandHandler(
        ICompanyRepository repository,
        MethodResultFactory methodResultFactory) 
    : IRequestHandler<DeleteCompanyCommand, MethodResult<MessageResponse>>
{
    public async Task<MethodResult<MessageResponse>> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<MessageResponse>();

        var company = await repository.GetByIdAsync(request.Id);
        if (company is null)
        {
            result.SetError($"Company with ID {request.Id} not found", HttpStatusCode.NotFound);
            return result;
        }

        await repository.DeleteAsync(company);

        result.Data = new MessageResponse("Company was successfully deleted");
        
        return result;
    }
}