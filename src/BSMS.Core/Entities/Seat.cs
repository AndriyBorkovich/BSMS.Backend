namespace BSMS.Core.Entities;

public class Seat
{
    public int SeatId { get; set; }
    public int BusId { get; set; }
    public int SeatNumber { get; set; }
    public bool IsFree { get; set; }

    public Bus Bus { get; set; }
    public Ticket? Ticket { get; set; }
}
