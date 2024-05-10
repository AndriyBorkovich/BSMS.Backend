using BSMS.Application.Contracts.Persistence;
using MapsterMapper;
using MediatR;

namespace BSMS.Application.Features.Company.Queries.GetTopByRevenue;

public record GetTopCompaniesByRevenueQuery() : IRequest<List<GetTopCompaniesByRevenueResponse>>;

public record GetTopCompaniesByRevenueResponse(
    string CompanyName,
    decimal Revenue
);

public class GetTopCompaniesByRevenueQueryHandler(
    ICompanyRepository repository,
    IMapper mapper) 
        : IRequestHandler<GetTopCompaniesByRevenueQuery, List<GetTopCompaniesByRevenueResponse>>
{
    public async Task<List<GetTopCompaniesByRevenueResponse>> Handle(GetTopCompaniesByRevenueQuery request, CancellationToken cancellationToken)
    {
        return mapper.Map<List<GetTopCompaniesByRevenueResponse>>(await repository.GetTopCompaniesByRevenueAsync(5));
    }
}