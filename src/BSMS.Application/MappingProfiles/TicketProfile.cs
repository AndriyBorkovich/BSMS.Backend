using BSMS.Application.Features.Ticket.Commands.Create;
using BSMS.Core.Entities;
using Mapster;

namespace BSMS.Application.MappingProfiles;

public class TicketProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateTicketCommand, Ticket>()
            .AfterMapping((_, dest) =>
            {
                dest.IsSold = false;
            });
    }
}