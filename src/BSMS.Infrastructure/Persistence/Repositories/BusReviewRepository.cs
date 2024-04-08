using BSMS.Application.Contracts.Persistence;
using BSMS.Core.Entities;

namespace BSMS.Infrastructure.Persistence.Repositories;

public class BusReviewRepository : GenericRepository<BusReview>, IBusReviewRepository
{
    public BusReviewRepository(BusStationContext context) : base(context)
    {
    }
}