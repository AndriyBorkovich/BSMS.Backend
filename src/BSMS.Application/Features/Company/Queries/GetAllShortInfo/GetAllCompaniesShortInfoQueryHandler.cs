using BSMS.Application.Contracts.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BSMS.Application.Features.Company.Queries.GetAllShortInfo;


public record GetAllCompaniesShortInfoQuery() 
    : IRequest<List<GetAllCompaniesShortInfoResponse>>;

public record GetAllCompaniesShortInfoResponse(
    int CompanyId,
    string Name
);

public class GetAllCompaniesShortInfoQueryHandler(
    ICompanyRepository repository
)
    : IRequestHandler<GetAllCompaniesShortInfoQuery, List<GetAllCompaniesShortInfoResponse>>
{
    public async Task<List<GetAllCompaniesShortInfoResponse>> Handle(GetAllCompaniesShortInfoQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetAll()
                               .AsNoTracking()
                               .ProjectToType<GetAllCompaniesShortInfoResponse>()
                               .ToListAsync(cancellationToken);
    }
}