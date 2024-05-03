using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Extensions;
using BSMS.Application.Features.Common;
using BSMS.Core.Enums;
using BSMS.Core.Views;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BSMS.Application.Features.Trip.Queries.GetAll;

public record GetAllTripsQuery(
    string? SearchedRoute,
    string? SearchedStatus,
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

        if(request.SearchedStatus is not null)
        {
            filters = filters.And(t => t.TripStatus == request.SearchedStatus);
        }

        var (trips, count) = await repository
                .GetDetails()
                .AsNoTracking()
                .Where(filters)
                .Where(t => t.DepartureTime.Date == todayDateTime.Date)
                .OrderBy(t => t.DepartureTime)
                .ProjectToType<GetAllTripsQueryRespone>()
                .Page(request.Pagination);

        return new ListResponse<GetAllTripsQueryRespone>(trips, count);
    }
}
