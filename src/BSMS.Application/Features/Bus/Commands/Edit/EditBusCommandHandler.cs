using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Extensions;
using BSMS.Application.Features.Common;
using BSMS.Application.Helpers;
using FluentValidation;
using MapsterMapper;
using MediatR;

namespace BSMS.Application.Features.Bus.Commands.Edit;
public record EditBusCommand(
    int BusId,
    string Brand,
    string Number,
    int Capacity) : IRequest<MethodResult<MessageResponse>>;

public class EditBusCommandHandler(
    IBusRepository repository,
    IMapper mapper,
    IValidator<EditBusCommand> validator,
    MethodResultFactory methodResultFactory) 
        : IRequestHandler<EditBusCommand, MethodResult<MessageResponse>>
{
    public async Task<MethodResult<MessageResponse>> Handle(EditBusCommand request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<MessageResponse>();

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            result.SetError(validationResult.Errors.ToResponse(), System.Net.HttpStatusCode.BadRequest);
            return result;
        }

        var bus = await repository.GetByIdAsync(request.BusId);

        mapper.Map<EditBusCommand, Core.Entities.Bus>(request, bus);

        await repository.UpdateAsync(bus);

        result.Data = new MessageResponse("Bus was edited successfully");

        return result;
    }
}