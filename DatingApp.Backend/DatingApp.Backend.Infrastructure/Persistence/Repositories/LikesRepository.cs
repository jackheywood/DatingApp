using DatingApp.Backend.Application.Contracts.Persistence.Repositories;
using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Domain.Entities;
using DatingApp.Backend.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Backend.Infrastructure.Persistence.Repositories;

public class LikesRepository(DatingAppDbContext context) : AsyncRepository<UserLike>(context), ILikesRepository
{
    public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
    {
        return await Context.Likes.FindAsync(sourceUserId, targetUserId);
    }

    public async Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId)
    {
        var users = Context.Users.Where(u => u.Id < 0);
        var likes = Context.Likes.AsQueryable();

        switch (predicate)
        {
            case "liked":
                likes = likes.Where(l => l.SourceUserId == userId);
                users = likes.Select(l => l.TargetUser);
                break;
            case "likedBy":
                likes = likes.Where(l => l.TargetUserId == userId);
                users = likes.Select(l => l.SourceUser);
                break;
        }

        return await users.OrderBy(u => u.Username).Select(user => new LikeDto
            {
                Id = user.Id,
                Username = user.Username,
                Age = user.DateOfBirth.CalculateAge(),
                KnownAs = user.KnownAs,
                City = user.City,
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
            })
            .ToListAsync();
    }
}