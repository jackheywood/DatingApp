using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.Backend.Application.Contracts.Persistence.Repositories;
using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.Helpers;
using DatingApp.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Backend.Infrastructure.Persistence.Repositories;

public class UserRepository(DatingAppDbContext context, IMapper mapper) : IUserRepository
{
    public async Task<MemberDto> GetMemberAsync(string username)
    {
        return await context.Users
            .Where(u => u.Username == username)
            .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
    {
        var query = context.Users
            .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
            .AsNoTracking();

        return await PagedList<MemberDto>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
    }

    public async Task<AppUser> GetByIdAsync(int id)
    {
        return await context.Users.SingleOrDefaultAsync(u => u.Id == id);
    }

    public async Task<AppUser> GetByUsernameAsync(string username)
    {
        return await context.Users
            .Include(u => u.Photos)
            .SingleOrDefaultAsync(UsernameMatches(username));
    }

    public async Task<IEnumerable<AppUser>> GetAllAsync()
    {
        return await context.Users.ToListAsync();
    }

    public async Task AddAsync(AppUser user)
    {
        await context.AddAsync(user);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void Update(AppUser user)
    {
        context.Entry(user).State = EntityState.Modified;
    }

    public async Task<bool> ExistsAsync(string username)
    {
        return await context.Users.AnyAsync(UsernameMatches(username));
    }

    private static Expression<Func<AppUser, bool>> UsernameMatches(string username)
    {
        return u => u.Username == username;
    }
}