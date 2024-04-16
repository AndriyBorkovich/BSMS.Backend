using BSMS.Core.Entities;
using BSMS.Core.Enums;
using BSMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BSMS.Infrastructure.Authorization.Services;

public class UserService : IUserService
{
    private readonly BusStationContext _context;

    public UserService(BusStationContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user)
    {
        _context.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateLastLoginDateAsync(User user)
    {
        user.LastLoginDate = DateTime.UtcNow;
        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByIdAsync(int userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<bool> ExistsAsync(string loginName)
    {
        return await _context.Users.AnyAsync(u => u.Username == loginName || u.Email == loginName);
    }

    public void RegisterWithDbRole(string userName, string password, Role role)
    {
        _context.Database.ExecuteSqlInterpolated($"EXEC dbo.CreateUserWithRole {userName}, {password}, {role}");
    }
}