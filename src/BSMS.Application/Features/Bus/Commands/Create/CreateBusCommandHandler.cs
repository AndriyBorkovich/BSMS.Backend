using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Features.Common;
using BSMS.Application.Helpers;
using MapsterMapper;
using MediatR;

namespace BSMS.Application.Features.Bus.Commands.Create;

public class CreateBusCommandHandler(
        IBusRepository busRepository, 
        IMapper mapper,
        MethodResultFactory methodResultFactory)
    : IRequestHandler<CreateBusCommand, MethodResult<CreatedEntityResponse>>
{
    public async Task<MethodResult<CreatedEntityResponse>> Handle(CreateBusCommand request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<CreatedEntityResponse>();
        var bus = mapper.Map<Core.Entities.Bus>(request);

        await busRepository.InsertAsync(bus);

        result.Data = new CreatedEntityResponse(bus.BusId);
        
        return result;
    }
}