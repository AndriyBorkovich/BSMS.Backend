using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLengthToStringColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusScheduleEntry_Buses_BusId",
                table: "BusScheduleEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_BusScheduleEntry_Routes_RouteId",
                table: "BusScheduleEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketPayment_Passengers_PassengerId",
                table: "TicketPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketPayment_Tickets_TicketId",
                table: "TicketPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketPayment_Trips_TripId",
                table: "TicketPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_BusScheduleEntry_BusScheduleEntryId",
                table: "Trips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketPayment",
                table: "TicketPayment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusScheduleEntry",
                table: "BusScheduleEntry");

            migrationBuilder.RenameTable(
                name: "TicketPayment",
                newName: "TicketPayments");

            migrationBuilder.RenameTable(
                name: "BusScheduleEntry",
                newName: "BusScheduleEntries");

            migrationBuilder.RenameIndex(
                name: "IX_TicketPayment_TripId",
                table: "TicketPayments",
                newName: "IX_TicketPayments_TripId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketPayment_TicketId",
                table: "TicketPayments",
                newName: "IX_TicketPayments_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketPayment_PassengerId",
                table: "TicketPayments",
                newName: "IX_TicketPayments_PassengerId");

            migrationBuilder.RenameIndex(
                name: "IX_BusScheduleEntry_RouteId",
                table: "BusScheduleEntries",
                newName: "IX_BusScheduleEntries_RouteId");

            migrationBuilder.RenameIndex(
                name: "IX_BusScheduleEntry_BusId",
                table: "BusScheduleEntries",
                newName: "IX_BusScheduleEntries_BusId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Stops",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Origin",
                table: "Routes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Destination",
                table: "Routes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Passengers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Passengers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Passengers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Passengers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Drivers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Drivers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DriverLicense",
                table: "Drivers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Companies",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ContactPhone",
                table: "Companies",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactEmail",
                table: "Companies",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Companies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "BusReviews",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Buses",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Brand",
                table: "Buses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MoveDirection",
                table: "BusScheduleEntries",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Day",
                table: "BusScheduleEntries",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketPayments",
                table: "TicketPayments",
                column: "TicketPaymentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusScheduleEntries",
                table: "BusScheduleEntries",
                column: "BusScheduleEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusScheduleEntries_Buses_BusId",
                table: "BusScheduleEntries",
                column: "BusId",
                principalTable: "Buses",
                principalColumn: "BusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusScheduleEntries_Routes_RouteId",
                table: "BusScheduleEntries",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "RouteId",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPayments_Trips_TripId",
                table: "TicketPayments",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "TripId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_BusScheduleEntries_BusScheduleEntryId",
                table: "Trips",
                column: "BusScheduleEntryId",
                principalTable: "BusScheduleEntries",
                principalColumn: "BusScheduleEntryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusScheduleEntries_Buses_BusId",
                table: "BusScheduleEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_BusScheduleEntries_Routes_RouteId",
                table: "BusScheduleEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketPayments_Passengers_PassengerId",
                table: "TicketPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketPayments_Tickets_TicketId",
                table: "TicketPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketPayments_Trips_TripId",
                table: "TicketPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_BusScheduleEntries_BusScheduleEntryId",
                table: "Trips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketPayments",
                table: "TicketPayments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusScheduleEntries",
                table: "BusScheduleEntries");

            migrationBuilder.RenameTable(
                name: "TicketPayments",
                newName: "TicketPayment");

            migrationBuilder.RenameTable(
                name: "BusScheduleEntries",
                newName: "BusScheduleEntry");

            migrationBuilder.RenameIndex(
                name: "IX_TicketPayments_TripId",
                table: "TicketPayment",
                newName: "IX_TicketPayment_TripId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketPayments_TicketId",
                table: "TicketPayment",
                newName: "IX_TicketPayment_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketPayments_PassengerId",
                table: "TicketPayment",
                newName: "IX_TicketPayment_PassengerId");

            migrationBuilder.RenameIndex(
                name: "IX_BusScheduleEntries_RouteId",
                table: "BusScheduleEntry",
                newName: "IX_BusScheduleEntry_RouteId");

            migrationBuilder.RenameIndex(
                name: "IX_BusScheduleEntries_BusId",
                table: "BusScheduleEntry",
                newName: "IX_BusScheduleEntry_BusId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Stops",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Origin",
                table: "Routes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Destination",
                table: "Routes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Passengers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Passengers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Passengers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Passengers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "DriverLicense",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ContactPhone",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactEmail",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "BusReviews",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Buses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Brand",
                table: "Buses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "MoveDirection",
                table: "BusScheduleEntry",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Day",
                table: "BusScheduleEntry",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketPayment",
                table: "TicketPayment",
                column: "TicketPaymentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusScheduleEntry",
                table: "BusScheduleEntry",
                column: "BusScheduleEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusScheduleEntry_Buses_BusId",
                table: "BusScheduleEntry",
                column: "BusId",
                principalTable: "Buses",
                principalColumn: "BusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusScheduleEntry_Routes_RouteId",
                table: "BusScheduleEntry",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "RouteId",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_BusScheduleEntry_BusScheduleEntryId",
                table: "Trips",
                column: "BusScheduleEntryId",
                principalTable: "BusScheduleEntry",
                principalColumn: "BusScheduleEntryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
