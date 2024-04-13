using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSeatsGenerationOnBusCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE GenerateSeatsForBus
                    @BusId INT,
                    @SeatCount INT
                AS
                BEGIN
                    DECLARE @SeatNumber INT = 1;

                    WHILE @SeatNumber <= @SeatCount
                    BEGIN
                        INSERT INTO Seats (BusId, SeatNumber, IsFree)
                        VALUES (@BusId, @SeatNumber, 1); -- Assuming all seats are initially free

                        SET @SeatNumber = @SeatNumber + 1;
                    END;
                END;
                GO

                CREATE TRIGGER GenerateSeatsOnBusInsert
                ON Buses
                AFTER INSERT
                AS
                BEGIN
                    DECLARE @BusId INT;

                    SELECT @BusId = inserted.BusId FROM inserted;

                    DECLARE @SeatCount INT;
                    SELECT @SeatCount = Capacity FROM inserted;

                    EXEC GenerateSeatsForBus @BusId, @SeatCount;
                END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP TRIGGER IF EXISTS GenerateSeatsOnBusInsert ON Buses;
                DROP PROCEDURE IF EXISTS GenerateSeatsForBus;");
        }
    }
}
