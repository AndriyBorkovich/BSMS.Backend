using BSMS.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BSMS.Application.Features.Ticket.Queries.GetTicketDistibutionByPaymentType;
public record GetTicketDistibutionByPaymentTypeQuery() : IRequest<List<GetTicketDistibutionByPaymentTypeResponse>>;
public class GetTicketDistibutionByPaymentTypeResponse
{
    public string TypeName { get; set; }
    public int Count { get; set; }
}
public class GetTicketDistibutionByPaymentTypeQueryHandler(
    ITicketPaymentRepository repository
) : IRequestHandler<GetTicketDistibutionByPaymentTypeQuery, List<GetTicketDistibutionByPaymentTypeResponse>>
{
    public async Task<List<GetTicketDistibutionByPaymentTypeResponse>> Handle(GetTicketDistibutionByPaymentTypeQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetAll()
                                .AsNoTracking()
                                .GroupBy(tp => tp.PaymentType)
                                .Select(g => new GetTicketDistibutionByPaymentTypeResponse
                                {
                                    TypeName = g.Key.ToString(),
                                    Count = g.Count()
                                })
                            .ToListAsync(cancellationToken);
    }
}