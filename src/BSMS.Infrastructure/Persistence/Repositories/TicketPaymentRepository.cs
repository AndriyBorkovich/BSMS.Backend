using BSMS.Application.Contracts.Persistence;
using BSMS.Core.Entities;
using BSMS.Core.Views;

namespace BSMS.Infrastructure.Persistence.Repositories;

public class TicketPaymentRepository : GenericRepository<TicketPayment>, ITicketPaymentRepository
{
    public TicketPaymentRepository(BusStationContext context) : base(context)
    {
    }

    public IQueryable<PaymentsView> GetPayments()
    {
        return Context.PaymentsView;
    }
}