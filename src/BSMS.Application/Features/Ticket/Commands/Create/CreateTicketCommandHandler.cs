using System.Net;
using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Extensions;
using BSMS.Application.Features.Common;
using BSMS.Application.Helpers;
using BSMS.Core.Entities;
using FluentValidation;
using MapsterMapper;
using MediatR;

namespace BSMS.Application.Features.Ticket.Commands.Create;

public record CreateTicketCommand(
    int SeatId,
    int StartStopId,
    int EndStopId,
    decimal Price) : IRequest<MethodResult<CreatedEntityResponse>>;

public class CreateTicketCommandHandler(
    ITicketRepository repository,
    IValidator<CreateTicketCommand> validator,
    IMapper mapper,
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

        var ticket = mapper.Map<Core.Entities.Ticket>(request);
        // 'Active' ticket status creation is handled by sql trigger
        await repository.InsertAsync(ticket);

        result.Data = new CreatedEntityResponse(ticket.TicketId);
        return result;
    }
}