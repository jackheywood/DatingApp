using System.Linq.Expressions;
using DatingApp.Backend.Application.Contracts.Repositories;
using DatingApp.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Backend.Infrastructure.Data.Repositories;

public class UserRepository(DatingAppDbContext context) : IUserRepository
{
    public async Task<AppUser> GetByIdAsync(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<AppUser> GetByUsernameAsync(string username)
    {
        return await context.Users.SingleOrDefaultAsync(UsernameMatches(username));
    }

    public async Task<IReadOnlyList<AppUser>> ListAllAsync()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<AppUser> AddAsync(AppUser user)
    {
        await context.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> ExistsAsync(string username)
    {
        return await context.Users.AnyAsync(UsernameMatches(username));
    }

    private static Expression<Func<AppUser, bool>> UsernameMatches(string username)
    {
        return u => u.Username.ToLower() == username.ToLower();
    }
}
