using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Helpers;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BSMS.Application.Extensions;
using BSMS.Application.Features.Common;

namespace BSMS.Application.Features.Bus.Queries.GetAll;

public record GetAllBusesQuery(
    string? SearchedBrand,
    string? SearchedBusNumber,
    Pagination Pagination) : IRequest<MethodResult<ListResponse<GetAllBusesResponse>>>;

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
    : IRequestHandler<GetAllBusesQuery, MethodResult<ListResponse<GetAllBusesResponse>>>
{
    public async Task<MethodResult<ListResponse<GetAllBusesResponse>>> Handle(
        GetAllBusesQuery request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<ListResponse<GetAllBusesResponse>>();

        var query = repository.GetBusesDetails().AsNoTracking();
        
        if (!string.IsNullOrWhiteSpace(request.SearchedBrand))
        {
            query = query.Where(b => b.Brand.StartsWith(request.SearchedBrand));
        }

        if (!string.IsNullOrWhiteSpace(request.SearchedBusNumber))
        {
            query = query.Where(b => b.Number.StartsWith(request.SearchedBusNumber));
        }
        
        var (busesList, total) = await query.ProjectToType<GetAllBusesResponse>()
                                            .Page(request.Pagination);

        result.Data = new ListResponse<GetAllBusesResponse>(busesList, total);

        return result;
    }
}