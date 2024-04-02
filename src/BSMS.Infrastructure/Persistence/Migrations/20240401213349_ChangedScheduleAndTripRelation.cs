using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangedScheduleAndTripRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trips_BusScheduleEntryId",
                table: "Trips");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_BusScheduleEntryId",
                table: "Trips",
                column: "BusScheduleEntryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trips_BusScheduleEntryId",
                table: "Trips");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_BusScheduleEntryId",
                table: "Trips",
                column: "BusScheduleEntryId",
                unique: true);
        }
    }
}
