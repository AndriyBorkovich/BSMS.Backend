using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Helpers;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BSMS.Application.Features.Passenger.Queries.GetAll;

public record GetAllPassengersQuery(
    string? SearchedFirstName,
    string? SearchedLastName) : IRequest<MethodResult<List<GetAllPassengersResponse>>>;

public record GetAllPassengersResponse(
    int PassengerId,
    string FullName,
    string PhoneNumber,
    string Email);

public class GetAllPassengersQueryHandler(
        IPassengerRepository repository,
        MethodResultFactory methodResultFactory)
    : IRequestHandler<GetAllPassengersQuery, MethodResult<List<GetAllPassengersResponse>>>
{
    public async Task<MethodResult<List<GetAllPassengersResponse>>> Handle(
        GetAllPassengersQuery request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<List<GetAllPassengersResponse>>();

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

        result.Data = await query.Where(filters)
                            .ProjectToType<GetAllPassengersResponse>()
                            .ToListAsync(cancellationToken);
        
        return result;
    }

    private static ExpressionStarter<Core.Entities.Passenger> SetUpFilters(GetAllPassengersQuery request)
    {
        var filters = PredicateBuilder.New<Core.Entities.Passenger>(true);
        if (!string.IsNullOrWhiteSpace(request.SearchedFirstName))
        {
            filters = filters.Or(p => p.FirstName.StartsWith(request.SearchedFirstName));
        }

        if (!string.IsNullOrWhiteSpace(request.SearchedLastName))
        {
            filters = filters.Or(p => p.LastName.StartsWith(request.SearchedLastName));
        }

        return filters;
    }
}