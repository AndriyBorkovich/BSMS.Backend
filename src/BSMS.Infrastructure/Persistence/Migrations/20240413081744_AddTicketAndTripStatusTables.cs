using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketAndTripStatusTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketPayments_Passengers_PassengerId",
                table: "TicketPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketPayments_Tickets_TicketId",
                table: "TicketPayments");

            migrationBuilder.DropIndex(
                name: "IX_TicketPayments_TicketId",
                table: "TicketPayments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "TicketStatus",
                table: "TicketPayments");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "TicketPayments",
                newName: "PaymentDate");

            migrationBuilder.AlterColumn<int>(
                name: "TicketId",
                table: "TicketPayments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PassengerId",
                table: "TicketPayments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TicketStatus",
                columns: table => new
                {
                    TicketStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    StatusTicketStatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketStatus", x => x.TicketStatusId);
                    table.ForeignKey(
                        name: "FK_TicketStatus_TicketStatus_StatusTicketStatusId",
                        column: x => x.StatusTicketStatusId,
                        principalTable: "TicketStatus",
                        principalColumn: "TicketStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketStatus_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TripStatus",
                columns: table => new
                {
                    TripStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    StatusTripStatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripStatus", x => x.TripStatusId);
                    table.ForeignKey(
                        name: "FK_TripStatus_TripStatus_StatusTripStatusId",
                        column: x => x.StatusTripStatusId,
                        principalTable: "TripStatus",
                        principalColumn: "TripStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TripStatus_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "TripId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketPayments_TicketId",
                table: "TicketPayments",
                column: "TicketId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketStatus_StatusTicketStatusId",
                table: "TicketStatus",
                column: "StatusTicketStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketStatus_TicketId",
                table: "TicketStatus",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TripStatus_StatusTripStatusId",
                table: "TripStatus",
                column: "StatusTripStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TripStatus_TripId",
                table: "TripStatus",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPayments_Passengers_PassengerId",
                table: "TicketPayments",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "PassengerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPayments_Tickets_TicketId",
                table: "TicketPayments",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketPayments_Passengers_PassengerId",
                table: "TicketPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketPayments_Tickets_TicketId",
                table: "TicketPayments");

            migrationBuilder.DropTable(
                name: "TicketStatus");

            migrationBuilder.DropTable(
                name: "TripStatus");

            migrationBuilder.DropIndex(
                name: "IX_TicketPayments_TicketId",
                table: "TicketPayments");

            migrationBuilder.RenameColumn(
                name: "PaymentDate",
                table: "TicketPayments",
                newName: "Date");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Trips",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "TicketId",
                table: "TicketPayments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PassengerId",
                table: "TicketPayments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TicketStatus",
                table: "TicketPayments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TicketPayments_TicketId",
                table: "TicketPayments",
                column: "TicketId",
                unique: true,
                filter: "[TicketId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPayments_Passengers_PassengerId",
                table: "TicketPayments",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "PassengerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPayments_Tickets_TicketId",
                table: "TicketPayments",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "TicketId");
        }
    }
}
