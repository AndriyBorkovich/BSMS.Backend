using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRelationBetweenBusAndPassenger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_Buses_BusId",
                table: "Passengers");

            migrationBuilder.DropIndex(
                name: "IX_Passengers_BusId",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "BusId",
                table: "Passengers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusId",
                table: "Passengers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_BusId",
                table: "Passengers",
                column: "BusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_Buses_BusId",
                table: "Passengers",
                column: "BusId",
                principalTable: "Buses",
                principalColumn: "BusId");
        }
    }
}
