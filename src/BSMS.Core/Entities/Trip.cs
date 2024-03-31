using BSMS.Core.Enums;

namespace BSMS.Core.Entities;

public class Trip
{
    public int TripId { get; set; }
    public TripStatus Status { get; set; }
    public int RouteId { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    
    public Route Route { get; set; }
    public List<Bus> Buses { get; set; }
}
