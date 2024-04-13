using BSMS.Application.Contracts.Persistence;
using BSMS.Core.Entities;

namespace BSMS.Infrastructure.Persistence.Repositories;

public class TripRepository : GenericRepository<Trip>, ITripRepository
{
    public TripRepository(BusStationContext context) : base(context)
    {
    }
}