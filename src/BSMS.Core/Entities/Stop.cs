namespace BSMS.Core.Entities;

public class Stop
{
    public int StopId { get; set; }
    public int? RouteId { get; set; }
    public int? PreviousStopId { get; set; }
    
    public Stop? PreviousStop { get; set; }
    public Route? Route { get; set; }
    public List<Ticket> TicketEndStops { get; set; }
    public List<Ticket> TicketStartStops { get; set; }
}
