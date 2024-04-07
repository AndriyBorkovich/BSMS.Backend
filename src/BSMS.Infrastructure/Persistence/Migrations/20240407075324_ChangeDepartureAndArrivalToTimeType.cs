using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDepartureAndArrivalToTimeType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeOnly>(
                name: "DepartureTime",
                table: "BusScheduleEntries",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "ArrivalTime",
                table: "BusScheduleEntries",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DepartureTime",
                table: "BusScheduleEntries",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ArrivalTime",
                table: "BusScheduleEntries",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time");
        }
    }
}
