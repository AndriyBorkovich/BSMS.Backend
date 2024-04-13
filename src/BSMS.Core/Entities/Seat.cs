namespace BSMS.Core.Entities;

public class Seat
{
    public int SeatId { get; set; }
    public int BusId { get; set; }
    
    public int SeatNumber { get; set; }
    public bool IsFree { get; set; }

    public Bus Bus { get; set; }
    /// <summary>
    /// represents fact that seat can have many tickets because of many bus trips
    /// </summary>
    public List<Ticket> Tickets { get; set; }
}
