using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentsView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW PaymentsView
                AS
                SELECT tp.TicketPaymentId,
                    tp.PaymentDate,
                    tp.PaymentType,
                    (p.FirstName + ' ' + p.LastName) AS BoughtBy,
                    t.Price,
                    tr.RouteName,
                    ss.Name AS StartStop,
                    es.Name AS EndStop
                FROM TicketPayments as tp
                JOIN Tickets AS t ON tp.TicketId = t.TicketId
                JOIN Passengers AS p ON tp.PassengerId = p.PassengerId
                JOIN TripView AS tr ON tp.TripId = tr.TripId
                JOIN Stops AS ss ON t.StartStopId = ss.StopId
                JOIN Stops AS es ON t.EndStopId = es.StopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP VIEW IF EXISTS PaymentsView");
        }
    }
}
