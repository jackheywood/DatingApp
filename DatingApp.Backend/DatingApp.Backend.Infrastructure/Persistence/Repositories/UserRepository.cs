using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.Backend.Application.Contracts.Persistence.Repositories;
using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.Helpers;
using DatingApp.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Backend.Infrastructure.Persistence.Repositories;

public class UserRepository(DatingAppDbContext context, IMapper mapper)
    : AsyncRepository<AppUser>(context), IUserRepository
{
    public async Task<AppUser> GetByIdAsync(int userId)
    {
        return await Context.Users.SingleOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<AppUser> GetByUsernameAsync(string username)
    {
        return await Context.Users
            .Include(u => u.Photos)
            .SingleOrDefaultAsync(UsernameMatches(username));
    }

    public async Task<AppUser> GetUserWithLikes(int userId)
    {
        return await Context.Users.Include(u => u.LikedUsers)
            .SingleOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<MemberDto> GetMemberAsync(string username)
    {
        return await Context.Users
            .Where(u => u.Username == username)
            .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
    {
        var query = Context.Users.AsQueryable();

        query = query.Where(u => u.Username != userParams.CurrentUsername);
        query = query.Where(u => u.Gender == userParams.Gender);

        var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
        var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

        query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

        query = userParams.OrderBy switch
        {
            "created" => query.OrderByDescending(u => u.Created),
            _ => query.OrderByDescending(u => u.LastActive),
        };

        return await PagedList<MemberDto>.CreateAsync(
            query.ProjectTo<MemberDto>(mapper.ConfigurationProvider).AsNoTracking(),
            userParams.PageNumber,
            userParams.PageSize);
    }

    public async Task<bool> ExistsAsync(string username)
    {
        return await Context.Users.AnyAsync(UsernameMatches(username));
    }

    private static Expression<Func<AppUser, bool>> UsernameMatches(string username)
    {
        return u => u.Username == username;
    }
}