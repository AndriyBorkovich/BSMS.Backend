using BSMS.Application.Contracts.Persistence;
using BSMS.Core.Entities;

namespace BSMS.Infrastructure.Persistence.Repositories;

public class StopRepository : GenericRepository<Stop>, IStopRepository
{
    public StopRepository(BusStationContext context) : base(context)
    {
    }

    public bool StopsBelongToSameRoute(int firstStopId, int secondStopId)
    {
        // workaround to call SQL function with EF
        return Context.Stops.Select(_ => Context.StopsBelongToSameRoute(firstStopId, secondStopId)).FirstOrDefault();
    }
}