using System.Linq.Expressions;

namespace BSMS.Application.Contracts.Persistence;

public interface IGenericRepository<T> where T: class
{
    IQueryable<T> GetAll();
    Task<T?> GetByIdAsync(int id);
    Task InsertAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<bool> AnyAsync(Expression<Func<T, bool>> conditions);
}