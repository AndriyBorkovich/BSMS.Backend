using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Helpers;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BSMS.Application.Features.Bus.Queries.GetAll;

public record GetAllBusesQuery(
    string? SearchedBrand,
    string? SearchedBusNumber) : IRequest<MethodResult<List<GetAllBusesResponse>>>;

public record GetAllBusesResponse(
    int BusId,
    string Number,
    string Brand,
    int Capacity,
    string? DriverName,
    string? CompanyName,
    double Rating);

public class GetAllBusesQueryHandler(
        IBusRepository repository,
        ILogger<GetAllBusesQuery> logger,
        MethodResultFactory methodResultFactory) 
    : IRequestHandler<GetAllBusesQuery, MethodResult<List<GetAllBusesResponse>>>
{
    public async Task<MethodResult<List<GetAllBusesResponse>>> Handle(GetAllBusesQuery request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<List<GetAllBusesResponse>>();

        var query = repository
            .GetBusesDetails()
            .AsNoTracking();
        
        if (!string.IsNullOrWhiteSpace(request.SearchedBrand))
        {
            query = query.Where(b => b.Brand.StartsWith(request.SearchedBrand));
        }

        if (!string.IsNullOrWhiteSpace(request.SearchedBusNumber))
        {
            query = query.Where(b => b.Number.StartsWith(request.SearchedBusNumber));
        }
        
        result.Data = await query.ProjectToType<GetAllBusesResponse>().ToListAsync(cancellationToken);
        
        return result;
    }
}