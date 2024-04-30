using System.ComponentModel.DataAnnotations;

namespace BSMS.Core.Entities;

public class Stop
{
    public int StopId { get; set; }
    public int RouteId { get; set; }
    public int? PreviousStopId { get; set; }
    
    [StringLength(50)]
    public string Name { get; set; }
    public int? DistanceToPrevious { get; set; }
    
    public Stop? PreviousStop { get; set; }
    public Route Route { get; set; }
    public List<Ticket> TicketEndStops { get; set; }
    public List<Ticket> TicketStartStops { get; set; }
}
