using System.Linq.Expressions;
using BSMS.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BSMS.Infrastructure.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T>
    where T: class
{
    protected readonly BusStationContext Context;
    public GenericRepository(BusStationContext context)
    {
        Context = context;
    }

    public IQueryable<T> GetAll()
    {
        return Context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await Context.Set<T>().FindAsync(id);
    }

    public async Task InsertAsync(T entity)
    {
        await Context.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task BulkInsertAsync(IEnumerable<T> entitites)
    {
        await Context.BulkInsertAsync(entitites);
    }

    public async Task UpdateAsync(T entity)
    {
        Context.Entry(entity).State = EntityState.Modified;
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        Context.Remove(entity);
        await Context.SaveChangesAsync();
    }
    
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> conditions)
    {
        return await Context.Set<T>().AnyAsync(conditions);
    }
}