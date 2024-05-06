using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Helpers;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BSMS.Application.Extensions;
using BSMS.Application.Features.Common;
using LinqKit;
using BSMS.Core.Views;
using BSMS.Application.Contracts.Caching;

namespace BSMS.Application.Features.Bus.Queries.GetAll;

public record GetAllBusesQuery(
    string? SearchedBrand,
    string? SearchedBusNumber,
    bool? HaveBoughtTickets,
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
        ICacheService cacheService,
        MethodResultFactory methodResultFactory)
    : IRequestHandler<GetAllBusesQuery, MethodResult<ListResponse<GetAllBusesResponse>>>
{
    public async Task<MethodResult<ListResponse<GetAllBusesResponse>>> Handle(
        GetAllBusesQuery request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<ListResponse<GetAllBusesResponse>>();

        result.Data = await TryGetFromCacheAsync(request, cancellationToken);

        return result;
    }

    private async Task<ListResponse<GetAllBusesResponse>> TryGetFromCacheAsync(
        GetAllBusesQuery request, CancellationToken cancellationToken)
    {
        var key = GetKey(request);
        var cachedBuses = await cacheService.GetRecordAsync<ListResponse<GetAllBusesResponse>>(key, cancellationToken);

        if (cachedBuses is not null)
        {
            logger.LogInformation("Got buses list from cache");
            return cachedBuses;
        }

        var query = repository.GetBusesDetails().AsNoTracking();

        var filters = await SetUpFilters(request);

        var (busesList, total) = await query.Where(filters)
                                            .ProjectToType<GetAllBusesResponse>()
                                            .Page(request.Pagination);

        var result = new ListResponse<GetAllBusesResponse>(busesList, total);

        await cacheService.SetRecordAsync(
            key,
            result,
            absoluteExpireTime: TimeSpan.FromMinutes(10),
            cancellationToken: cancellationToken);

        logger.LogInformation("Got buses list from DB and cached it");

        return result;
    }

    private static string GetKey(GetAllBusesQuery request)
    {
        var (from, to) = PaginationExtensions.GetFromAndToParams(request.Pagination);

        return $"{CachePrefixConstants.BusesKey}({from}-{to})_{request.SearchedBrand}_{request.SearchedBusNumber}";
    }

    private async Task<ExpressionStarter<BusDetailsView>> SetUpFilters(GetAllBusesQuery request)
    {
        var filters = PredicateBuilder.New<BusDetailsView>(true);
        if (!string.IsNullOrWhiteSpace(request.SearchedBrand))
        {
            filters = filters.And(b => b.Brand.StartsWith(request.SearchedBrand));
        }

        if (!string.IsNullOrWhiteSpace(request.SearchedBusNumber))
        {
            filters = filters.And(b => b.Number.StartsWith(request.SearchedBusNumber));
        }

        if (request.HaveBoughtTickets is not null and true)
        {
            var busIdsWithBoughtTickets = await repository.GetAll()
                        .AsNoTracking()
                        .Where(b => b.BusScheduleEntries.Any(bs => bs.Trips.Any(t => t.BoughtTickets.Count != 0)))
                        .Select(b => b.BusId)
                        .ToListAsync();

            filters = filters.And(b => busIdsWithBoughtTickets.Contains(b.BusId));
        }

        return filters;
    }
}