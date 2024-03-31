using BSMS.Application.Contracts.Persistence;
using BSMS.Core.Entities;

namespace BSMS.Infrastructure.Persistence.Repositories;

public class BusRepository: GenericRepository<Bus>, IBusRepository
{
    public BusRepository(BusStationContext context) : base(context)
    {
    }
}