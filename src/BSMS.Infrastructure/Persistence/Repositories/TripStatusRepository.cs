using BSMS.Application.Contracts.Persistence;
using BSMS.Core.Entities;

namespace BSMS.Infrastructure.Persistence.Repositories;

public class TripStatusRepository : GenericRepository<TripStatus>, ITripStatusRepository
{
    public TripStatusRepository(BusStationContext context) : base(context)
    {
    }
}