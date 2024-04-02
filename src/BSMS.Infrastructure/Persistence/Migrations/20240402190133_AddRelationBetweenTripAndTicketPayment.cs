using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationBetweenTripAndTicketPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketPayment_Passengers_PassengerId",
                table: "TicketPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketPayment_Tickets_TicketId",
                table: "TicketPayment");

            migrationBuilder.DropIndex(
                name: "IX_TicketPayment_TicketId",
                table: "TicketPayment");

            migrationBuilder.AlterColumn<int>(
                name: "TicketId",
                table: "TicketPayment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PassengerId",
                table: "TicketPayment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TripId",
                table: "TicketPayment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TicketPayment_TicketId",
                table: "TicketPayment",
                column: "TicketId",
                unique: true,
                filter: "[TicketId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPayment_TripId",
                table: "TicketPayment",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPayment_Passengers_PassengerId",
                table: "TicketPayment",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "PassengerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPayment_Tickets_TicketId",
                table: "TicketPayment",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPayment_Trips_TripId",
                table: "TicketPayment",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "TripId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketPayment_Passengers_PassengerId",
                table: "TicketPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketPayment_Tickets_TicketId",
                table: "TicketPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketPayment_Trips_TripId",
                table: "TicketPayment");

            migrationBuilder.DropIndex(
                name: "IX_TicketPayment_TicketId",
                table: "TicketPayment");

            migrationBuilder.DropIndex(
                name: "IX_TicketPayment_TripId",
                table: "TicketPayment");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "TicketPayment");

            migrationBuilder.AlterColumn<int>(
                name: "TicketId",
                table: "TicketPayment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PassengerId",
                table: "TicketPayment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketPayment_TicketId",
                table: "TicketPayment",
                column: "TicketId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPayment_Passengers_PassengerId",
                table: "TicketPayment",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "PassengerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPayment_Tickets_TicketId",
                table: "TicketPayment",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
