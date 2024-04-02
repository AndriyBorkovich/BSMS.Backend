using System.ComponentModel.DataAnnotations;

namespace BSMS.Core.Entities;

public class Passenger
{
    public int PassengerId { get; set; }
    public int? BusId { get; set; }
    
    [StringLength(50)]
    public string FirstName { get; set; } = null!;
    [StringLength(50)]
    public string LastName { get; set; } = null!;
    [StringLength(20)]
    public string? PhoneNumber { get; set; }
    [StringLength(50)]
    public string? Email { get; set; }
    
    public Bus? Bus { get; set; }
    public List<BusReview> BusReviews { get; set; }
    public List<TicketPayment> Payments { get; set; }
}
