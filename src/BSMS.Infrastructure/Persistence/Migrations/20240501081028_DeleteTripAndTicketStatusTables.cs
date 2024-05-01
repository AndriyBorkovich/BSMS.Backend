using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DeleteTripAndTicketStatusTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP TRIGGER IF EXISTS HandleSeatStateOnStatusInsert;
                DROP TRIGGER IF EXISTS AddActiveTicketStatus");
            migrationBuilder.DropTable(
                name: "TicketStatuses");

            migrationBuilder.DropTable(
                name: "TripStatuses");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DepartureTime",
                table: "Trips",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ArrivalTime",
                table: "Trips",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Trips",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Tickets",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
                CREATE OR ALTER TRIGGER [dbo].[HandleSeatStateOnTicketInsert]
                ON [dbo].[Tickets]
                AFTER INSERT
                AS
                BEGIN
                    DECLARE @TicketId INT, @SeatId INT;
                    DECLARE @Status NVARCHAR(20);

                    SELECT @TicketId = inserted.TicketId, @Status = inserted.Status
				    FROM inserted;

					EXEC HandleSeatAvailability @TicketId, @Status;
                END;
                GO
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER TRIGGER [dbo].[HandleSeatStateOnTicketUpdate]
                ON [dbo].[Tickets]
                AFTER UPDATE
                AS
                BEGIN
                    DECLARE @TicketId INT, @SeatId INT;
                    DECLARE @Status NVARCHAR(20);

                    SELECT @TicketId = updated.TicketId, @Status = updated.Status
				    FROM updated;

					EXEC HandleSeatAvailability @TicketId, @Status;
                END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP TRIGGER IF EXISTS HandleSeatStateOnTicketUpdate;
                DROP TRIGGER IF EXISTS HandleSeatStateOnTicketInsert;");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tickets");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DepartureTime",
                table: "Trips",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ArrivalTime",
                table: "Trips",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TicketStatuses",
                columns: table => new
                {
                    TicketStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketStatuses", x => x.TicketStatusId);
                    table.ForeignKey(
                        name: "FK_TicketStatuses_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId");
                });

            migrationBuilder.CreateTable(
                name: "TripStatuses",
                columns: table => new
                {
                    TripStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripStatuses", x => x.TripStatusId);
                    table.ForeignKey(
                        name: "FK_TripStatuses_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "TripId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketStatuses_TicketId",
                table: "TicketStatuses",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TripStatuses_TripId",
                table: "TripStatuses",
                column: "TripId");

            migrationBuilder.Sql(@"
                CREATE OR ALTER TRIGGER [dbo].[AddActiveTicketStatus]
                ON [dbo].[Tickets]
                AFTER INSERT
                AS
                BEGIN
                    DECLARE @TicketId INT, @SeatId INT;
                    DECLARE @CreatedDate DATETIME = GETUTCDATE();
                    DECLARE @Status NVARCHAR(20) = 'Active';

                    SELECT @TicketId = inserted.TicketId, @Status = Status
				    FROM inserted;

                    INSERT INTO TicketStatuses (TicketId, CreatedDate, Status)
                    VALUES (@TicketId, @CreatedDate, @Status);
                END;
                GO");

            migrationBuilder.Sql(@"
                ALTER TRIGGER [dbo].[HandleSeatStateOnStatusInsert]
                ON [dbo].[TicketStatuses]
                AFTER INSERT
                AS
                BEGIN
	                DECLARE @TicketId INT;
	                DECLARE @NewStatus NVARCHAR(20);

	                SELECT @TicketId = inserted.TicketId,
		                   @NewStatus = inserted.Status
	                FROM inserted;

	                EXEC HandleSeatAvailability @TicketId, @NewStatus;

                END;");
        }
    }
}
