using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Features.Common;
using BSMS.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BSMS.Infrastructure.Persistence.Repositories;

public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
{
    public CompanyRepository(BusStationContext context) : base(context)
    {
    }
    
     public async Task<List<CompanyRevenue>> GetTopCompaniesByRevenueAsync(int count)
    {
        return await Context.Companies
            .Join(
                Context.Drivers,
                company => company.CompanyId,
                driver => driver.CompanyId,
                (company, driver) => new { Company = company, Driver = driver })
            .Join(
                Context.Buses,
                cd => cd.Driver.DriverId,
                bus => bus.DriverId,
                (cd, bus) => new { cd.Company, Bus = bus })
            .Join(
                Context.BusScheduleEntries,
                cdBus => cdBus.Bus.BusId,
                bse => bse.BusId,
                (cdBus, bse) => new { cdBus.Company, BusScheduleEntry = bse })
            .Join(
                Context.Trips,
                cdBusBse => cdBusBse.BusScheduleEntry.BusScheduleEntryId,
                trip => trip.BusScheduleEntryId,
                (cdBusBse, trip) => new { cdBusBse.Company, Trip = trip })
            .Join(
                Context.TicketPayments,
                cdBusBseTrip => cdBusBseTrip.Trip.TripId,
                tp => tp.TripId,
                (cdBusBseTrip, tp) => new { cdBusBseTrip.Company, TicketPayment = tp })
            .Join(
                Context.Tickets,
                cdBusBseTripTp => cdBusBseTripTp.TicketPayment.TicketId,
                ticket => ticket.TicketId,
                (cdBusBseTripTp, ticket) => new { cdBusBseTripTp.Company, Ticket = ticket })
            .GroupBy(
                cdBusBseTripTp => cdBusBseTripTp.Company.Name,
                (key, group) => new CompanyRevenue
                {
                    CompanyName = key,
                    Revenue = group.Sum(cdBusBseTripTp => cdBusBseTripTp.Ticket.Price)
                })
            .OrderByDescending(result => result.Revenue)
            .Take(count)
            .ToListAsync();
    }
}