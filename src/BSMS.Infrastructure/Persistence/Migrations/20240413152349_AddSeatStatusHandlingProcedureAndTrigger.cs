using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSeatStatusHandlingProcedureAndTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROC HandleSeatAvailability
                (   @TicketId INT,
                    @NewStatus NVARCHAR(20))
                AS
                BEGIN
	                -- set seat state based on new ticket status
	                DECLARE @IsFree BIT;

                    -- check new status
                   IF @NewStatus = 'Active' OR @NewStatus = 'Cancelled'
                        SET @IsFree = 1;
                    ELSE
                        SET @IsFree = 0;

                    -- update corresponding seat
                    UPDATE Seats
                    SET IsFree = @IsFree
                    WHERE SeatId = (SELECT SeatId FROM Tickets WHERE TicketId = @TicketId);
                END;
                GO

                CREATE TRIGGER HandleSeatStateOnStatusInsert
                ON TicketStatuses
                AFTER INSERT
                AS
                BEGIN
	                DECLARE @TicketId INT;
	                DECLARE @NewStatus NVARCHAR(20);

	                SELECT @TicketId = inserted.TicketId,
		                   @NewStatus = inserted.Status
	                FROM inserted;

	                EXEC HandleSeatAvailability @TicketId, @NewStatus;

                END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP TRIGGER IF EXISTS HandleSeatStateOnStatusInsert;
                DROP PROCEDURE IF EXISTS HandleSeatAvailability;");
        }
    }
}
