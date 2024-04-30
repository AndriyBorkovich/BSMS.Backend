using BSMS.Core.Entities;

namespace BSMS.Application.Contracts.Persistence;

public interface IStopRepository : IGenericRepository<Stop>
{
    bool StopsBelongToSameRoute(int firstStopId, int secondStopId);

    void ExecuteCustomizedInsert(int routeId, string stopName, int distanceToPrevious);
}