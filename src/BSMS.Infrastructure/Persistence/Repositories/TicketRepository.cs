using BSMS.Application.Contracts.Persistence;
using BSMS.Core.Entities;

namespace BSMS.Infrastructure.Persistence.Repositories;

public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
{
    public TicketRepository(BusStationContext context) : base(context)
    {
    }
}