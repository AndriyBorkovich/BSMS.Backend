using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketPriceCalculations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER FUNCTION CalculateTicketPrice(@StopId INT)
                RETURNS DECIMAL(18,2)
                AS
                BEGIN
                    DECLARE  @RouteId INT;
                    SELECT @RouteId = RouteId FROM Stops Where StopId = @StopId;

                    RETURN dbo.CalculateTotalDistanceForRoute(@RouteId) * 0.3
                END");
            
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Tickets",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "dbo.CalculateTicketPrice([EndStopId])",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Tickets",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "dbo.CalculateTicketPrice([EndStopId])");
            
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS CalculateTicketPrice");
        }
    }
}
