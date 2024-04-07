using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRelationBetweenBusAndDriver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Buses_BusId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_BusId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "BusId",
                table: "Drivers");

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "Buses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Buses_DriverId",
                table: "Buses",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buses_Drivers_DriverId",
                table: "Buses",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "DriverId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buses_Drivers_DriverId",
                table: "Buses");

            migrationBuilder.DropIndex(
                name: "IX_Buses_DriverId",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Buses");

            migrationBuilder.AddColumn<int>(
                name: "BusId",
                table: "Drivers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_BusId",
                table: "Drivers",
                column: "BusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Buses_BusId",
                table: "Drivers",
                column: "BusId",
                principalTable: "Buses",
                principalColumn: "BusId");
        }
    }
}
