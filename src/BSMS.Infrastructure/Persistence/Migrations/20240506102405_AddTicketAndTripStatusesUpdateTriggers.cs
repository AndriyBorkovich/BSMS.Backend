using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketAndTripStatusesUpdateTriggers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE TRIGGER UpdateTripsTicketsStatus
                ON Trips
                AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;
                    IF UPDATE(Status) AND EXISTS(SELECT 1 FROM Trips WHERE Status IN('InTransit', 'Completed', 'Canceled'))
                    BEGIN
                        UPDATE tk
                        SET Status = 
                            CASE 
                                WHEN t.Status = 'InTransit' THEN 'InUse'
                                WHEN t.Status = 'Completed' THEN 'Used'
                                WHEN t.Status = 'Canceled' THEN 'Cancelled'
                                ELSE tk.Status -- Preserve the current status if not modified
                            END
                        FROM Tickets AS tk
                        INNER JOIN TicketPayments AS tp ON tk.TicketId = tp.TicketId
                        INNER JOIN Trips AS t ON tp.TripId = t.TripId
                        WHERE tp.TripId = t.TripId;
                    END
                END;
                GO

                CREATE TRIGGER UpdateTicketSeatStatus
                ON Tickets
                AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;

                    IF UPDATE(Status)
                    BEGIN
                        IF EXISTS (
                            SELECT 1
                            FROM Tickets
                            WHERE Status = 'InUse'
                        )
                        BEGIN
                            UPDATE Seats
                            SET IsFree = 0
                            WHERE SeatId IN (
                                SELECT s.SeatId
                                FROM Seats s
                                INNER JOIN Tickets t ON s.SeatId = t.SeatId
                                WHERE t.Status = 'InUse'
                            );
                        END
                        ELSE
                        BEGIN
                            UPDATE Seats
                            SET IsFree = 1
                            WHERE SeatId IN (
                                SELECT s.SeatId
                                FROM Seats s
                                INNER JOIN Tickets t ON s.SeatId = t.SeatId
                                WHERE t.Status <> 'InUse'
                            );
                        END
                    END
                END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP TRIGGER IF EXISTS UpdateTripsTicketsStatus;
            DROP TRIGGER IF EXISTS UpdateTicketSeatStatus;
            DROP TRIGGER IF EXISTS HandleSeatStateOnTicketUpdate;");
        }
    }
}
