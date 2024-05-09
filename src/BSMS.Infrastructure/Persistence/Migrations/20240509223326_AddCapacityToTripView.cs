using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCapacityToTripView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            create or alter view TripView
                WITH SCHEMABINDING AS
                select
                    t.TripId,
                    t.DepartureTime,
                    t.ArrivalTime,
                    r.Origin + ' - ' + r.Destination as RouteName,
                    b.Brand as BusBrand,
                    b.CompanyName,
                    b.Rating as BusRating,
                    t.Status as TripStatus,
                    dbo.CalculateFreeSeats(t.TripId) as FreeSeatsCount,
					b.Capacity
                from dbo.Trips as t
                    join dbo.BusScheduleEntries as bse
                        on t.BusScheduleEntryId = bse.BusScheduleEntryId
                    join dbo.Routes as r
                        on bse.RouteId = r.RouteId
                    join dbo.BusDetailsView as b
                        on bse.BusId = b.BusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            create or alter view TripView
                WITH SCHEMABINDING AS
                select
                    t.TripId,
                    t.DepartureTime,
                    t.ArrivalTime,
                    r.Origin + ' - ' + r.Destination as RouteName,
                    b.Brand as BusBrand,
                    b.CompanyName,
                    b.Rating as BusRating,
                    t.Status as TripStatus,
                    dbo.CalculateFreeSeats(t.TripId) as FreeSeatsCount
                from dbo.Trips as t
                    join dbo.BusScheduleEntries as bse
                        on t.BusScheduleEntryId = bse.BusScheduleEntryId
                    join dbo.Routes as r
                        on bse.RouteId = r.RouteId
                    join dbo.BusDetailsView as b
                        on bse.BusId = b.BusId");
        }
    }
}
