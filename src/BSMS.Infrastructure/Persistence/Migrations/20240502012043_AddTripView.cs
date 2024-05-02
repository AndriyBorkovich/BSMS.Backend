using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTripView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER FUNCTION dbo.CalculateFreeSeats(@tripId INT)
                RETURNS INT
                WITH SCHEMABINDING
                AS
                BEGIN
                    DECLARE @totalSeats INT;
                    DECLARE @boughtTicketsCount INT;

                    -- Calculate total number of seats for the bus associated with the trip
                    SELECT @totalSeats = b.Capacity
                    FROM dbo.Trips t
                    INNER JOIN dbo.BusScheduleEntries bse ON t.BusScheduleEntryId = bse.BusScheduleEntryId
                    INNER JOIN dbo.Buses b ON bse.BusId = b.BusId
                    WHERE t.TripId = @tripId;

                    -- Calculate count of bought tickets for the trip
                    SELECT @boughtTicketsCount = COUNT(*)
                    FROM dbo.TicketPayments
                    WHERE TripId = @tripId;

                    RETURN @totalSeats - @boughtTicketsCount;
                END;
                GO");

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
                        on bse.BusId = b.BusId
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP VIEW IF EXISTS TripView
                DROP FUNCTION IF EXISTS CalculateFreeSeats;");
        }
    }
}
