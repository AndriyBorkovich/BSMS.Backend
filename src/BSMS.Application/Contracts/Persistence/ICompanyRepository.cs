using BSMS.Application.Features.Common;
using BSMS.Core.Entities;

namespace BSMS.Application.Contracts.Persistence;

public interface ICompanyRepository : IGenericRepository<Company>
{
    Task<List<CompanyRevenue>> GetTopCompaniesByRevenueAsync(int count);
}