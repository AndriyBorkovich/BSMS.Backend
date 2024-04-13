using BSMS.Core.Enums;

namespace BSMS.Core.Entities;
/// <summary>
/// represent trip history (live) for specific bus on specific route
/// </summary>
public class Trip
{
    public int TripId { get; set; }
    public int BusScheduleEntryId { get; set; }
    
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    
    public BusScheduleEntry BusScheduleEntry { get; set; }
    public List<TicketPayment> BoughtTickets { get; set; }
    public List<TripStatus> Statuses { get; set; }
}
