using BSMS.Application.Contracts.Persistence;
using BSMS.Core.Entities;

namespace BSMS.Infrastructure.Persistence.Repositories;

public class SeatRepository : GenericRepository<Seat>, ISeatRepository
{
    public SeatRepository(BusStationContext context) : base(context)
    {
    }
}