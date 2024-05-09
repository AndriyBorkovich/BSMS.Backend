using System.Net;
using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Extensions;
using BSMS.Application.Features.Common;
using BSMS.Application.Helpers;
using BSMS.Core.Entities;
using BSMS.Core.Enums;
using FluentValidation;
using MediatR;

namespace BSMS.Application.Features.Ticket.Commands.Create;

public record CreateTicketCommand(
    int SeatId,
    int StartStopId,
    int EndStopId,
    int TripId,
    int PassengerId,
    PaymentType PaymentType) : IRequest<MethodResult<CreatedEntityResponse>>;

public class CreateTicketCommandHandler(
    ITicketRepository repository,
    IValidator<CreateTicketCommand> validator,
    MethodResultFactory methodResultFactory) 
        : IRequestHandler<CreateTicketCommand, MethodResult<CreatedEntityResponse>>
{
    public async Task<MethodResult<CreatedEntityResponse>> Handle(
        CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<CreatedEntityResponse>();
        
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            result.SetError(validationResult.Errors.ToResponse(), HttpStatusCode.BadRequest);
            return result;
        }

        var ticket = new Core.Entities.Ticket
        {
            SeatId = request.SeatId,
            StartStopId = request.StartStopId,
            EndStopId = request.EndStopId,
            IsSold = true,
            Status = TicketStatus.InUse,
            Payment = new TicketPayment
            {
                TripId = request.TripId,
                PassengerId = request.PassengerId,
                PaymentDate = DateTime.Now,
                PaymentType = request.PaymentType
            }
        };
        
        await repository.InsertAsync(ticket);

        result.Data = new CreatedEntityResponse(ticket.TicketId);
        return result;
    }
}