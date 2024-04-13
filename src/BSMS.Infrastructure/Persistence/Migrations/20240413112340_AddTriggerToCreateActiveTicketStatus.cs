using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTriggerToCreateActiveTicketStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE TRIGGER AddActiveTicketStatus
                ON Tickets
                AFTER INSERT
                AS
                BEGIN
                    DECLARE @TicketId INT, @SeatId INT;
                    DECLARE @CreatedDate DATETIME = GETUTCDATE();
                    DECLARE @Status NVARCHAR(20) = 'Active';

                    SELECT @TicketId = inserted.TicketId
				    FROM inserted;

                    INSERT INTO TicketStatuses (TicketId, CreatedDate, Status)
                    VALUES (@TicketId, @CreatedDate, @Status);
                END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS AddActiveTicketStatus;");
        }
    }
}
