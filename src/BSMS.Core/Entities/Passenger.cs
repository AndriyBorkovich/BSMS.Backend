namespace BSMS.Core.Entities;

public class Passenger
{
    public int PassengerId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public int? BusId { get; set; }

    public Bus? Bus { get; set; }
    public List<BusReview> BusReviews { get; set; }
    public List<TicketPayment> Payments { get; set; }
}
