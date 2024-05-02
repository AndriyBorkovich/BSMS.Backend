using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexesForDepartureAndArrivalTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BusScheduleEntries_ArrivalTime",
                table: "BusScheduleEntries",
                column: "ArrivalTime");

            migrationBuilder.CreateIndex(
                name: "IX_BusScheduleEntries_DepartureTime",
                table: "BusScheduleEntries",
                column: "DepartureTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BusScheduleEntries_ArrivalTime",
                table: "BusScheduleEntries");

            migrationBuilder.DropIndex(
                name: "IX_BusScheduleEntries_DepartureTime",
                table: "BusScheduleEntries");
        }
    }
}
