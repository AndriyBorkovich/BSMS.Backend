namespace BSMS.Core.Entities;
/// <summary>
/// represent ticket status chronology
/// </summary>
public class TicketStatus
{
    public int TicketStatusId { get; set; }
    public int TicketId { get; set; }
    
    public Enums.TicketStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    
    public Ticket Ticket { get; set; }
}