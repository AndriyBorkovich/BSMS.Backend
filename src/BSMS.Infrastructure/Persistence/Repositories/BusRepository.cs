using BSMS.Application.Contracts.Persistence;
using BSMS.Core.Entities;
using BSMS.Core.Views;

namespace BSMS.Infrastructure.Persistence.Repositories;

public class BusRepository: GenericRepository<Bus>, IBusRepository
{
    public BusRepository(BusStationContext context) : base(context)
    {
    }

    public IQueryable<BusDetailsView> GetBusesDetails()
    {
        return Context.BusesDetailsView;
    }
}