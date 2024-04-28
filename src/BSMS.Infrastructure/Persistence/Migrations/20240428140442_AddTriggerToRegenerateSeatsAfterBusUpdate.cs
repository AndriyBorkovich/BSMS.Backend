using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTriggerToRegenerateSeatsAfterBusUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER TRIGGER [dbo].[RegenerateSeatsOnBusUpdate]
                ON [dbo].[Buses]
                AFTER UPDATE
                AS
                BEGIN
                    IF UPDATE(Capacity)
                    BEGIN
                        DECLARE @BusId INT;
                        DECLARE @NewSeatCount INT;
                        DECLARE @OldSeatCount INT;

                        SELECT @BusId = i.BusId,
                            @NewSeatCount = i.Capacity,
                            @OldSeatCount = d.Capacity
                        FROM inserted i
                        INNER JOIN deleted d ON i.BusId = d.BusId;

                        -- Check if the seat count has changed
                        IF @NewSeatCount <> @OldSeatCount
                        BEGIN
                            -- Clear existing seats
                            DELETE FROM Seats WHERE BusId = @BusId;

                            -- Regenerate seats
                            EXEC GenerateSeatsForBus @BusId, @NewSeatCount;
                        END;
                    END;
                END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP TRIGGER IF EXISTS RegenerateSeatsOnBusUpdate");
        }
    }
}
