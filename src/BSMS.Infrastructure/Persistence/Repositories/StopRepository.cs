using BSMS.Application.Contracts.Persistence;
using BSMS.Core.Entities;
using Microsoft.EntityFrameworkCore;

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

    public void ExecuteCustomizedInsert(int routeId, string stopName, int distanceToPrevious)
    {
        Context.Database.ExecuteSqlInterpolated(@$"INSERT INTO Stops VALUES({routeId}, NULL, '{stopName}', {distanceToPrevious})");
    }
}