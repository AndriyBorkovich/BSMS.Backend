using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Extensions;
using BSMS.Application.Features.Common;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BSMS.Application.Features.Ticket.Queries.GetAllPayments;

public record GetAllTicketPaymentsQuery(
    Pagination Pagination
) : IRequest<ListResponse<GetAllTicketPaymentsResponse>>;

public class GetAllTicketPaymentsResponse
{
    public int TicketPaymentId { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentType { get; set; }
    public string BoughtBy { get; set; }
    public decimal Price { get; set; }
    public string RouteName { get; set; }
    public string StartStop { get; set; }
    public string EndStop { get; set; }
}

public class GetAllTicketPaymentsQueryHandler(
    ITicketPaymentRepository repository
) : IRequestHandler<GetAllTicketPaymentsQuery, ListResponse<GetAllTicketPaymentsResponse>>
{
    public async Task<ListResponse<GetAllTicketPaymentsResponse>> Handle(GetAllTicketPaymentsQuery request, CancellationToken cancellationToken)
    {
        var (items, total) = await repository.GetPayments()
                .AsNoTracking()
                .OrderByDescending(p => p.PaymentDate)
                .ProjectToType<GetAllTicketPaymentsResponse>()
                .Page(request.Pagination);
        
        return new ListResponse<GetAllTicketPaymentsResponse>(items, total);
    }
}