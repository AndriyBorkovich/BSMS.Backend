using BSMS.Application.Contracts.Persistence;
using BSMS.Core.Entities;

namespace BSMS.Infrastructure.Persistence.Repositories;

public class TicketPaymentRepository : GenericRepository<TicketPayment>, ITicketPaymentRepository
{
    public TicketPaymentRepository(BusStationContext context) : base(context)
    {
    }
}