using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Extensions;
using BSMS.Application.Features.Common;
using BSMS.Core.Views;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BSMS.Application.Features.Trip.Queries.GetAll;

public record GetAllTripsQuery(
    string? SearchedRoute,
    Pagination Pagination
) : IRequest<ListResponse<GetAllTripsQueryRespone>>;

public record GetAllTripsQueryRespone(
    int TripId,
    DateTime DepartureTime,
    DateTime ArrivalTime,
    string RouteName,
    string BusBrand,
    string CompanyName,
    int BusRating,
    string TripStatus,
    int FreeSeatsCount
);

public class GetAllTripsQueryHandler(ITripRepository repository)
    : IRequestHandler<GetAllTripsQuery, ListResponse<GetAllTripsQueryRespone>>
{
    public async Task<ListResponse<GetAllTripsQueryRespone>> Handle(
        GetAllTripsQuery request, CancellationToken cancellationToken)
    {
        var todayDateTime = DateTime.Now;
        var filters = PredicateBuilder.New<TripView>(true);
        if (!string.IsNullOrWhiteSpace(request.SearchedRoute))
        {
            filters = filters.And(t => t.RouteName.Contains(request.SearchedRoute));
        }

        var (trips, count) = await repository
                .GetDetails()
                .AsNoTracking()
                .Where(filters)
                .Where(t => t.DepartureTime.Date == todayDateTime.Date
                    && t.DepartureTime >= todayDateTime)
                .ProjectToType<GetAllTripsQueryRespone>()
                .Page(request.Pagination);

        return new ListResponse<GetAllTripsQueryRespone>(trips, count);
    }
}
