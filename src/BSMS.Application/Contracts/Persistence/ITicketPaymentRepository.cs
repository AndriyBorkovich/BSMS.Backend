using BSMS.Core.Entities;
using BSMS.Core.Views;

namespace BSMS.Application.Contracts.Persistence;

public interface ITicketPaymentRepository : IGenericRepository<TicketPayment>
{
    IQueryable<PaymentsView> GetPayments();
}