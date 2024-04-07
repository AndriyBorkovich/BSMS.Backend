using System.ComponentModel.DataAnnotations;
using BSMS.Core.Enums;

namespace BSMS.Core.Entities;

public class BusScheduleEntry
{
    [Key]
    public int BusScheduleEntryId { get; set; }
    public int BusId { get; set; }
    public int RouteId { get; set; }
    
    public TimeOnly DepartureTime { get; set; }
    public TimeOnly ArrivalTime { get; set; }
    public Direction MoveDirection { get; set; }
    public DayOfWeek Day { get; set; }
    
    public Bus Bus { get; set; }
    public Route Route { get; set; }
    public List<Trip> Trips { get; set; }
}