using BSMS.Core.Entities;
using BSMS.Core.Enums;

namespace BSMS.Infrastructure.Authorization.Services;

public interface IUserService
{
    Task AddAsync(User user);
    Task<User?> GetByIdAsync(int userId);
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> ExistsAsync(string loginName);
    Task UpdateLastLoginDateAsync(User user);
    void RegisterWithDbRole(string userName, string password, Role role);
}