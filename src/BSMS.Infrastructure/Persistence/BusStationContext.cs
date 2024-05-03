using BSMS.Core.Entities;
using BSMS.Core.Views;
using Microsoft.EntityFrameworkCore;

namespace BSMS.Infrastructure.Persistence;

public class BusStationContext : DbContext
{
    public BusStationContext()
    {
    }

    public BusStationContext(DbContextOptions<BusStationContext> options)
        : base(options)
    {
    }

    public DbSet<Bus> Buses { get; set; }
    public DbSet<BusReview> BusReviews { get; set; }
    public DbSet<BusScheduleEntry> BusScheduleEntries { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Passenger> Passengers { get; set; }
    public DbSet<Route> Routes { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Stop> Stops { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketPayment> TicketPayments { get; set; }
    public DbSet<Trip> Trips { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<BusDetailsView> BusesDetailsView { get; set; }
    public DbSet<TripView> TripView { get; set; }
    
    public bool StopsBelongToSameRoute(int stopId1, int stopId2)
        => throw new NotSupportedException();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // prevent from SaveChanges() exception caused by SQL trigger
        modelBuilder.Entity<Bus>()
            .ToTable(tb => tb.UseSqlOutputClause(false));
        modelBuilder.Entity<Ticket>()
            .ToTable(tb => tb.UseSqlOutputClause(false));
        modelBuilder.Entity<Route>()
            .ToTable(tb => tb.UseSqlOutputClause(false));
        modelBuilder.Entity<Stop>()
            .ToTable(tb => tb.UseSqlOutputClause(false));
        
        modelBuilder.Entity<Stop>()
            .HasMany(s => s.TicketStartStops)
            .WithOne(t => t.StartStop)
            .OnDelete(DeleteBehavior.ClientSetNull);
        
        modelBuilder.Entity<Stop>()
            .HasMany(s => s.TicketEndStops)
            .WithOne(t => t.EndStop)
            .OnDelete(DeleteBehavior.ClientSetNull);

        modelBuilder
            .Entity<BusScheduleEntry>()
            .Property(bs => bs.Day)
            .HasConversion<string>()
            .HasMaxLength(20);
        
        modelBuilder
            .Entity<BusScheduleEntry>()
            .Property(bs => bs.MoveDirection)
            .HasConversion<string>()
            .HasMaxLength(20);

        modelBuilder.Entity<BusDetailsView>()
            .ToView(nameof(BusDetailsView))
            .HasKey(b => b.BusId);

        modelBuilder.Entity<TicketPayment>()
            .Property(tp => tp.PaymentType)
            .HasConversion<string>()
            .HasMaxLength(20);

        modelBuilder.Entity<Ticket>()
            .Property(ts => ts.Status)
            .HasConversion<string>()
            .HasMaxLength(20);
        
        modelBuilder.Entity<Trip>()
            .Property(ts => ts.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Payment)
            .WithOne(p => p.Ticket)
            .OnDelete(DeleteBehavior.ClientCascade);

        
        modelBuilder.HasDbFunction(
                typeof(BusStationContext).GetMethod(nameof(StopsBelongToSameRoute),
                [typeof(int), typeof(int)])!)
            .HasName("StopsBelongToSameRoute");

        modelBuilder.Entity<Route>()
            .Property(r => r.OverallDistance)
            .HasComputedColumnSql("dbo.CalculateTotalDistanceForRoute([RouteId])");
        
        modelBuilder.Entity<Ticket>()
            .Property(t => t.Price)
            .HasComputedColumnSql("dbo.CalculateNewTicketPrice([StartStopId], [EndStopId])");

         modelBuilder.Entity<TripView>()
            .ToView(nameof(TripView))
            .HasKey(t => t.TripId);
    }
}
