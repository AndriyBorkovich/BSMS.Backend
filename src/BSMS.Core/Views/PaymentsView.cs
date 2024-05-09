using BSMS.Core.Enums;

namespace BSMS.Core.Views;

public class PaymentsView
{
    public int TicketPaymentId { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentType { get; set; }
    public string BoughtBy { get; set; }
    public decimal Price { get; set; }
    public string RouteName { get; set; }
    public string StartStop { get; set; }
    public string EndStop { get; set; }
}