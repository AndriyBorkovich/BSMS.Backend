using BSMS.Application.Features.Common;
using Microsoft.EntityFrameworkCore;

namespace BSMS.Application.Extensions;

public static class PaginationExtensions
{
    public static async Task<(List<T>, int)> Page<T>(this IQueryable<T> query, Pagination pagination)
    {
        var count = await query.CountAsync();

        var (from, to) = GetFromAndToParams(pagination);

        var items = await query.Skip(from).Take(to).ToListAsync();

        return (items, count);
    }

    public static (int From, int To) GetFromAndToParams(Pagination pagination) 
    {
        return ((pagination.PageNumber - 1) * pagination.PageSize, pagination.PageSize);
    }
}