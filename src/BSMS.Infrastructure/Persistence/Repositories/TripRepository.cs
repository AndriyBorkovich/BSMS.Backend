using BSMS.Application.Contracts.Persistence;
using BSMS.Core.Entities;
using BSMS.Core.Views;

namespace BSMS.Infrastructure.Persistence.Repositories;

public class TripRepository : GenericRepository<Trip>, ITripRepository
{
    public TripRepository(BusStationContext context) : base(context)
    {
    }

    public IQueryable<TripView> GetDetails()
    {
        return Context.TripView;
    }
}