using BSMS.Application.Contracts.Caching;
using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Extensions;
using BSMS.Application.Features.Common;
using BSMS.Application.Helpers;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BSMS.Application.Features.Company.Queries.GetAll;

public record GetAllCompaniesQuery(
    string? SearchedName,
    string? SearchedCity,
    string? SearchedCountry,
    string? SearchedZipCode,
    Pagination Pagination
) : IRequest<MethodResult<ListResponse<GetAllCompaniesResponse>>>;

public record GetAllCompaniesResponse(
    int CompanyId,
    string Name,
    string Address,
    string? Phone,
    string? Email
);
public class GetAllCompaniesQueryHandler(
    ICompanyRepository repository,
    ILogger<GetAllCompaniesQueryHandler> logger,
    ICacheService cacheService,
    MethodResultFactory methodResultFactory)
        : IRequestHandler<GetAllCompaniesQuery, MethodResult<ListResponse<GetAllCompaniesResponse>>>
{
    public async Task<MethodResult<ListResponse<GetAllCompaniesResponse>>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<ListResponse<GetAllCompaniesResponse>>();
        
        result.Data = await TryGetFromCacheAsync(request, cancellationToken);

        return result;
    }

    private async Task<ListResponse<GetAllCompaniesResponse>> TryGetFromCacheAsync(
        GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        var key = GetKey(request);
        var cachedPassengers = await cacheService.GetRecordAsync<ListResponse<GetAllCompaniesResponse>>(key, cancellationToken);

        if (cachedPassengers is not null)
        {
            logger.LogInformation("Got companies list from cache");
            return cachedPassengers;
        }

        var query = repository.GetAll().AsNoTracking();

        var filters = SetUpFilters(request);

        var (companiesList, total) = await query.Where(filters)
                                            .ProjectToType<GetAllCompaniesResponse>()
                                            .Page(request.Pagination);

        var result = new ListResponse<GetAllCompaniesResponse>(companiesList, total);

        await cacheService.SetRecordAsync(
            key,
            result,
            absoluteExpireTime: TimeSpan.FromMinutes(5),
            cancellationToken: cancellationToken);

        logger.LogInformation("Got companies list from DB and cached it");

        return result;
    }

    private static string GetKey(GetAllCompaniesQuery request)
    {
        var (from, to) = PaginationExtensions.GetFromAndToParams(request.Pagination);

        return $"{CachePrefixConstants.CompaniesKey}({from}-{to})_{request.SearchedName}_{request.SearchedCity}_{request.SearchedCountry}_{request.SearchedZipCode}";
    }

    private static ExpressionStarter<Core.Entities.Company> SetUpFilters(GetAllCompaniesQuery request)
    {
        var filters = PredicateBuilder.New<Core.Entities.Company>(true);
        
        if (!string.IsNullOrWhiteSpace(request.SearchedName))
        {
            filters = filters.And(c => c.Name.StartsWith(request.SearchedName));
        }

        if (!string.IsNullOrWhiteSpace(request.SearchedCity))
        {
            filters = filters.And(c => c.City.StartsWith(request.SearchedCity));
        }

        if (!string.IsNullOrWhiteSpace(request.SearchedCountry))
        {
            filters = filters.And(c => c.Country.StartsWith(request.SearchedCountry));
        }

        if (!string.IsNullOrWhiteSpace(request.SearchedZipCode))
        {
            filters = filters.And(c => c.ZipCode.StartsWith(request.SearchedZipCode));
        }

        return filters;
    }
}