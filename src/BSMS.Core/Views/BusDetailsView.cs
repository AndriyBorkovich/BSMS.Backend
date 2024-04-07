namespace BSMS.Core.Views;

public class BusDetailsView
{
    public int BusId { get; set; }
    public string Number { get; set; }
    public string Brand { get; set; }
    public int Capacity { get; set; }
    public string? DriverName { get; set; }
    public string? CompanyName { get; set; }
    public double Rating { get; set; }
}