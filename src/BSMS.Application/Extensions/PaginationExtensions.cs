using BSMS.Application.Features.Common;
using Microsoft.EntityFrameworkCore;

namespace BSMS.Application.Extensions;

public static class PaginationExtensions
{
    public static async Task<(List<T>, int)> Page<T>(this IQueryable<T> query, Pagination pagination)
    {
        var count = await query.CountAsync();

        var items = await query
                        .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                        .Take(pagination.PageSize)
                        .ToListAsync();

        return (items, count);
    }
}