using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Extensions;
using BSMS.Application.Features.Common;
using BSMS.Application.Helpers;
using FluentValidation;
using MapsterMapper;
using MediatR;

namespace BSMS.Application.Features.Passenger.Commands.Edit;

public record EditPassengerCommand(
    int PassengerId,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Email) : IRequest<MethodResult<MessageResponse>>;

public class EditPassengerCommandHandler(
    IPassengerRepository repository,
    IMapper mapper,
    IValidator<EditPassengerCommand> validator,
    MethodResultFactory methodResultFactory)
        : IRequestHandler<EditPassengerCommand, MethodResult<MessageResponse>>
{
    public async Task<MethodResult<MessageResponse>> Handle(EditPassengerCommand request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<MessageResponse>();

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            result.SetError(validationResult.Errors.ToResponse(), System.Net.HttpStatusCode.BadRequest);
            return result;
        }

        var passenger = await repository.GetByIdAsync(request.PassengerId);

        mapper.Map<EditPassengerCommand, Core.Entities.Passenger>(request, passenger);

        await repository.UpdateAsync(passenger);

        result.Data = new MessageResponse($"Passenger with ID {request.PassengerId} was edited successfully");

        return result;
    }
}
