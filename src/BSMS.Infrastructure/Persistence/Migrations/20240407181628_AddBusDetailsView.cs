using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBusDetailsView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE FUNCTION dbo.CalculateBusRating(@BusId INT)
                RETURNS FLOAT
                AS
                BEGIN
                    DECLARE @Rating FLOAT;
	                DECLARE @Count FLOAT = 5.0;
                    SELECT @Rating = AVG(ComfortRating + PunctualityRating + PriceQualityRatioRating + 
						                InternetConnectionRating + SanitaryConditionsRating) / @Count
                    FROM BusReviews
                    WHERE BusId = @BusId;
                    RETURN ISNULL(@Rating, 0);
                END;
                GO");

            migrationBuilder.Sql(@"
                    CREATE VIEW dbo.BusDetailsView
                    AS
                    SELECT 
                        b.BusId,
                        b.Number,
                        b.Brand,
	                    b.Capacity,
	                    CONCAT(d.FirstName, ' ', d.LastName) AS DriverName,
	                    c.Name AS CompanyName,
                        dbo.CalculateBusRating(b.BusId) AS Rating
                    FROM Buses AS b
	                    JOIN Drivers AS d
		                    ON b.DriverId = d.DriverId
	                    LEFT JOIN Companies AS c
		                    ON d.CompanyId = c.CompanyId;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP VIEW dbo.BusDetailsView
                DROP FUNCTION dbo.CalculateBusRating");
        }
    }
}
