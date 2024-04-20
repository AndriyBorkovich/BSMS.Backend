using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Helpers;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BSMS.Application.Features.Driver.Queries.GetAllFromCompany;

public record GetAllDriversFromCompanyQuery(int CompanyId)
    : IRequest<MethodResult<List<GetAllDriversFromCompanyResponse>>>;

public record GetAllDriversFromCompanyResponse(
    int DriverId,
    string Name);
public class GetAllDriversFromCompanyQueryHandler(
    IDriverRepository driverRepository,
    ICompanyRepository companyRepository,
    MethodResultFactory methodResultFactory
) : IRequestHandler<GetAllDriversFromCompanyQuery, MethodResult<List<GetAllDriversFromCompanyResponse>>>
{
    public async Task<MethodResult<List<GetAllDriversFromCompanyResponse>>> Handle(GetAllDriversFromCompanyQuery request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<List<GetAllDriversFromCompanyResponse>>();

        var companyExits = await companyRepository.AnyAsync(c => c.CompanyId == request.CompanyId);
        if (!companyExits)
        {
            result.SetError($"Company with ID {request.CompanyId} not found", System.Net.HttpStatusCode.NotFound);
            return result;
        }

        result.Data = await driverRepository.GetAll()
                                            .AsNoTracking()
                                            .Where(d => d.CompanyId == request.CompanyId)
                                            .ProjectToType<GetAllDriversFromCompanyResponse>()
                                            .ToListAsync(cancellationToken);

        return result;
    }
}
