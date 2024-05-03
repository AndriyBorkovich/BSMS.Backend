using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLogicOfTicketPriceCalc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE OR ALTER FUNCTION CalculateNewTicketPrice
            (@StartStopId INT, @EndStopId INT)
                RETURNS DECIMAL(18,2)
                AS
                BEGIN
                    DECLARE @Distance1 INT, @Distance2 INT;
                    SELECT @Distance1 = DistanceToPrevious FROM Stops Where StopId = @StartStopId;
					SELECT @Distance2 = DistanceToPrevious FROM Stops Where StopId = @EndStopId;

                    RETURN ABS(ISNULL(@Distance2, 1) - ISNULL(@Distance1, 0)) * 0.3
                END");
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Tickets",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "dbo.CalculateNewTicketPrice([StartStopId], [EndStopId])",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "dbo.CalculateTicketPrice([EndStopId])");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Tickets",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "dbo.CalculateTicketPrice([EndStopId])",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "dbo.CalculateNewTicketPrice([StartStopId], [EndStopId])");
        }
    }
}
