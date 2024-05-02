using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMainIndexesForSearchedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Trips_Status",
                table: "Trips",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_FirstName",
                table: "Passengers",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_LastName",
                table: "Passengers",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_FirstName",
                table: "Drivers",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_LastName",
                table: "Drivers",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_BusScheduleEntries_Day",
                table: "BusScheduleEntries",
                column: "Day");

            migrationBuilder.CreateIndex(
                name: "IX_Buses_Number",
                table: "Buses",
                column: "Number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trips_Status",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Passengers_FirstName",
                table: "Passengers");

            migrationBuilder.DropIndex(
                name: "IX_Passengers_LastName",
                table: "Passengers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_FirstName",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_LastName",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_BusScheduleEntries_Day",
                table: "BusScheduleEntries");

            migrationBuilder.DropIndex(
                name: "IX_Buses_Number",
                table: "Buses");
        }
    }
}
