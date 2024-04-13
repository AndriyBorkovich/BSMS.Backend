namespace BSMS.Core.Entities;
/// <summary>
/// represent trip status chronology
/// </summary>
public class TripStatus
{
    public int TripStatusId { get; set; }
    public int TripId { get; set; }
    
    public Enums.TripStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    
    public Trip Trip { get; set; }
}