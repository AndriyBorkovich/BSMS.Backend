using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBusScheduleRepresentationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Routes_RouteId",
                table: "Trips");

            migrationBuilder.DropTable(
                name: "BusTrip");

            migrationBuilder.DropIndex(
                name: "IX_Trips_RouteId",
                table: "Trips");

            migrationBuilder.RenameColumn(
                name: "RouteId",
                table: "Trips",
                newName: "BusScheduleEntryId");

            migrationBuilder.CreateTable(
                name: "BusScheduleEntry",
                columns: table => new
                {
                    BusScheduleEntryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusId = table.Column<int>(type: "int", nullable: false),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MoveDirection = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusScheduleEntry", x => x.BusScheduleEntryId);
                    table.ForeignKey(
                        name: "FK_BusScheduleEntry_Buses_BusId",
                        column: x => x.BusId,
                        principalTable: "Buses",
                        principalColumn: "BusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusScheduleEntry_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "RouteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trips_BusScheduleEntryId",
                table: "Trips",
                column: "BusScheduleEntryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusScheduleEntry_BusId",
                table: "BusScheduleEntry",
                column: "BusId");

            migrationBuilder.CreateIndex(
                name: "IX_BusScheduleEntry_RouteId",
                table: "BusScheduleEntry",
                column: "RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_BusScheduleEntry_BusScheduleEntryId",
                table: "Trips",
                column: "BusScheduleEntryId",
                principalTable: "BusScheduleEntry",
                principalColumn: "BusScheduleEntryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_BusScheduleEntry_BusScheduleEntryId",
                table: "Trips");

            migrationBuilder.DropTable(
                name: "BusScheduleEntry");

            migrationBuilder.DropIndex(
                name: "IX_Trips_BusScheduleEntryId",
                table: "Trips");

            migrationBuilder.RenameColumn(
                name: "BusScheduleEntryId",
                table: "Trips",
                newName: "RouteId");

            migrationBuilder.CreateTable(
                name: "BusTrip",
                columns: table => new
                {
                    BusesBusId = table.Column<int>(type: "int", nullable: false),
                    TripsTripId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusTrip", x => new { x.BusesBusId, x.TripsTripId });
                    table.ForeignKey(
                        name: "FK_BusTrip_Buses_BusesBusId",
                        column: x => x.BusesBusId,
                        principalTable: "Buses",
                        principalColumn: "BusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusTrip_Trips_TripsTripId",
                        column: x => x.TripsTripId,
                        principalTable: "Trips",
                        principalColumn: "TripId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trips_RouteId",
                table: "Trips",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_BusTrip_TripsTripId",
                table: "BusTrip",
                column: "TripsTripId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Routes_RouteId",
                table: "Trips",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "RouteId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
