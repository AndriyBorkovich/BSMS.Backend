using BSMS.Core.Entities;
using BSMS.Core.Views;

namespace BSMS.Application.Contracts.Persistence;

public interface ITripRepository : IGenericRepository<Trip>
{
    IQueryable<TripView> GetDetails();
}