using BSMS.Core.Entities;

namespace BSMS.Infrastructure.Authorization.Services;

public interface IUserService
{
    Task AddAsync(User user);
    Task<User?> GetByIdAsync(int userId);
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> ExistsAsync(string loginName);
}