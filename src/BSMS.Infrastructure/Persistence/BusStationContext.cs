using BSMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
    public DbSet<Company> Companies { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Passenger> Passengers { get; set; }
    public DbSet<Route> Routes { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Stop> Stops { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Trip> Trips { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Stop>()
            .HasMany(s => s.TicketStartStops)
            .WithOne(t => t.StartStop)
            .OnDelete(DeleteBehavior.ClientSetNull);
        
        modelBuilder.Entity<Stop>()
            .HasMany(s => s.TicketEndStops)
            .WithOne(t => t.EndStop)
            .OnDelete(DeleteBehavior.ClientSetNull);

        // modelBuilder
        //     .Entity<BusScheduleEntry>()
        //     .Property(bs => bs.Day)
        //     .HasConversion<string>();
        //
        // modelBuilder
        //     .Entity<BusScheduleEntry>()
        //     .Property(bs => bs.MoveDirection)
        //     .HasConversion<string>();
    }
}
