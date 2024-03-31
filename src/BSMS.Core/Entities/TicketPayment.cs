using BSMS.Core.Enums;

namespace BSMS.Core.Entities;

public class TicketPayment
{
    public int TicketPaymentId { get; set; }
    public int PassengerId { get; set; }
    public int TicketId { get; set;}
    public TicketStatus TicketStatus {get; set;}
    public PaymentType PaymentType {get;set;}
    public DateTime Date {get;set;}
    
    public Passenger Passenger { get; set; }
    public Ticket Ticket { get; set; }
}