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

namespace BSMS.Application.Features.Passenger.Queries.GetAll;

public record GetAllPassengersQuery(
    string? SearchedFirstName,
    string? SearchedLastName,
    Pagination Pagination) : IRequest<MethodResult<ListResponse<GetAllPassengersResponse>>>;

public record GetAllPassengersResponse(
    int PassengerId,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Email);

public class GetAllPassengersQueryHandler(
        IPassengerRepository repository,
        ICacheService cacheService,
        ILogger<GetAllPassengersQueryHandler> logger,
        MethodResultFactory methodResultFactory)
    : IRequestHandler<GetAllPassengersQuery, MethodResult<ListResponse<GetAllPassengersResponse>>>
{
    public async Task<MethodResult<ListResponse<GetAllPassengersResponse>>> Handle(
        GetAllPassengersQuery request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<ListResponse<GetAllPassengersResponse>>();
        
        result.Data = await TryGetFromCacheAsync(request, cancellationToken);

        return result;
    }

    private async Task<ListResponse<GetAllPassengersResponse>> TryGetFromCacheAsync(
        GetAllPassengersQuery request, CancellationToken cancellationToken)
    {
        var key = GetKey(request);
        var cachedPassengers = await cacheService.GetRecordAsync<ListResponse<GetAllPassengersResponse>>(key, cancellationToken);

        if (cachedPassengers is not null)
        {
            logger.LogInformation("Got passengers list from cache");
            return cachedPassengers;
        }

        var query = repository.GetAll()
            .AsNoTracking()
            .Select(p => new Core.Entities.Passenger
            {
                PassengerId = p.PassengerId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                PhoneNumber = p.PhoneNumber,
                Email = p.Email
            });

        var filters = SetUpFilters(request);

        var (passengersList, total) = await query.Where(filters)
                                            .ProjectToType<GetAllPassengersResponse>()
                                            .Page(request.Pagination);

        var result = new ListResponse<GetAllPassengersResponse>(passengersList, total);

        await cacheService.SetRecordAsync(
            key,
            result,
            absoluteExpireTime: TimeSpan.FromMinutes(5),
            cancellationToken: cancellationToken);

        logger.LogInformation("Got passengers list from DB and cached it");

        return result;
    }

    private static string GetKey(GetAllPassengersQuery request)
    {
        var (from, to) = PaginationExtensions.GetFromAndToParams(request.Pagination);

        return $"{CachePrefixConstants.PassengersKey}({from}-{to})_{request.SearchedFirstName}_{request.SearchedLastName}";
    }

    private static ExpressionStarter<Core.Entities.Passenger> SetUpFilters(GetAllPassengersQuery request)
    {
        var filters = PredicateBuilder.New<Core.Entities.Passenger>(true);
        if (!string.IsNullOrWhiteSpace(request.SearchedFirstName))
        {
            filters = filters.And(p => p.FirstName.StartsWith(request.SearchedFirstName));
        }

        if (!string.IsNullOrWhiteSpace(request.SearchedLastName))
        {
            filters = filters.And(p => p.LastName.StartsWith(request.SearchedLastName));
        }

        return filters;
    }
}