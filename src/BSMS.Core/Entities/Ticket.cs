namespace BSMS.Core.Entities;

public class Ticket
{
    public int TicketId { get; set; }
    public int? PassengerId { get; set; }
    public int SeatId { get; set; }
    public decimal Price { get; set; }
    public bool IsSold { get; set; }
    public int StartStopId { get; set; }
    public int EndStopId { get; set; }
    
    public Stop EndStop { get; set; } = null!;
    public Stop StartStop { get; set; } = null!;
    public Passenger? Passenger { get; set; }
    public Seat Seat { get; set; } = null!;
}
