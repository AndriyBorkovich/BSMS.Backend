﻿using BSMS.Application.Features.Common;
using BSMS.Core.Entities;
using BSMS.Core.Views;

namespace BSMS.Application.Contracts.Persistence;

public interface IBusRepository : IGenericRepository<Bus>
{
    IQueryable<BusDetailsView> GetBusesDetails();
    Task<List<BusDistance>> GetMostCrossedDistanceAsync();
}