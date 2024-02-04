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

    public async Task<bool> ExistsAsync(string userName)
    {
        return await context.Users.AnyAsync(u => u.UserName.ToLower() == userName.ToLower());
    }
}
