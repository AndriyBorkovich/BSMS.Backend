using BSMS.Application.Contracts.Caching;
using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Extensions;
using BSMS.Application.Features.Common;
using BSMS.Application.Helpers;
using BSMS.Core.Entities;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BSMS.Application.Features.Driver.Queries.GetAll;
public record GetAllDriversQuery(
    string? SearchedFirstName,
    string? SearchedLastName,
    string? SearchedLicense,
    Pagination Pagination
) : IRequest<MethodResult<ListResponse<GetAllDriversResponse>>>;

public record GetAllDriversResponse(
    int DriverId,
    string FirstName,
    string LastName,
    string License,
    string CompanyName
);

public class GetAllDriversQueryHandler(
    IDriverRepository repository,
    ILogger<GetAllDriversQueryHandler> logger,
    ICacheService cacheService,
    MethodResultFactory methodResultFactory)
        : IRequestHandler<GetAllDriversQuery, MethodResult<ListResponse<GetAllDriversResponse>>>
{
    public async Task<MethodResult<ListResponse<GetAllDriversResponse>>> Handle(GetAllDriversQuery request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<ListResponse<GetAllDriversResponse>>();
        result.Data = await TryGetFromCacheAsync(request, cancellationToken);
        return result;
    }

    private async Task<ListResponse<GetAllDriversResponse>> TryGetFromCacheAsync(GetAllDriversQuery request, CancellationToken cancellationToken)
    {
        var key = GetKey(request);
        var cachedDrivers = await cacheService.GetRecordAsync<ListResponse<GetAllDriversResponse>>(key, cancellationToken);

        if (cachedDrivers is not null)
        {
            logger.LogInformation("Got drivers list from cache");
            return cachedDrivers;
        }

        var query = repository.GetAll()
                              .AsNoTracking()
                              .Select(d => new Core.Entities.Driver
                              {
                                  DriverId = d.DriverId,
                                  FirstName = d.FirstName,
                                  LastName = d.LastName,
                                  DriverLicense = d.DriverLicense,
                                  Company = new Core.Entities.Company
                                  {
                                      Name = d.Company.Name
                                  }
                              });

        var filters = SetUpFilters(request);

        var (driversList, total) = await query.Where(filters)
                                            .ProjectToType<GetAllDriversResponse>()
                                            .Page(request.Pagination);

        var result = new ListResponse<GetAllDriversResponse>(driversList, total);

        await cacheService.SetRecordAsync(
            key,
            result,
            absoluteExpireTime: TimeSpan.FromMinutes(10),
            cancellationToken: cancellationToken);

        logger.LogInformation("Got drivers list from DB and cached it");

        return result;
    }

    private static string GetKey(GetAllDriversQuery request)
    {
        var (from, to) = PaginationExtensions.GetFromAndToParams(request.Pagination);

        return $"{CachePrefixConstants.DriversKey}({from}-{to})_{request.SearchedFirstName}_{request.SearchedLastName}_{request.SearchedLicense}";
    }

    private static ExpressionStarter<Core.Entities.Driver> SetUpFilters(GetAllDriversQuery request)
    {
        var filters = PredicateBuilder.New<Core.Entities.Driver>(true);
        if (!string.IsNullOrWhiteSpace(request.SearchedFirstName))
        {
            filters = filters.And(d => d.FirstName.StartsWith(request.SearchedFirstName));
        }

        if (!string.IsNullOrWhiteSpace(request.SearchedLastName))
        {
            filters = filters.And(d => d.LastName.StartsWith(request.SearchedLastName));
        }

        if (!string.IsNullOrWhiteSpace(request.SearchedLicense))
        {
            filters = filters.And(d => d.DriverLicense != null && d.DriverLicense.StartsWith(request.SearchedLicense));
        }

        return filters;
    }
}