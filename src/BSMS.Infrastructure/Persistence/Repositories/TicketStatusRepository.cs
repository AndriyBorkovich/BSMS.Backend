using BSMS.Application.Contracts.Persistence;
using BSMS.Core.Entities;

namespace BSMS.Infrastructure.Persistence.Repositories;

public class TicketStatusRepository : GenericRepository<TicketStatus>, ITicketStatusRepository
{
    public TicketStatusRepository(BusStationContext context) : base(context)
    {
    }
}