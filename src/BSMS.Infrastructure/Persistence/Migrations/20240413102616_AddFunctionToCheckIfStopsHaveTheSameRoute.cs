using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFunctionToCheckIfStopsHaveTheSameRoute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE FUNCTION StopsBelongToSameRoute
                (
                    @StopId1 INT,
                    @StopId2 INT
                )
                RETURNS BIT
                AS
                BEGIN
                    DECLARE @RouteId1 INT;
                    DECLARE @RouteId2 INT;

                    SELECT @RouteId1 = RouteId
                    FROM Stops
                    WHERE StopId = @StopId1;

                    SELECT @RouteId2 = RouteId
                    FROM Stops
                    WHERE StopId = @StopId2;
                    
                    IF @RouteId1 = @RouteId2
                    BEGIN
                        RETURN 1;
                    END
                    
                    RETURN 0;
                END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS StopsBelongToSameRoute");
        }
    }
}
