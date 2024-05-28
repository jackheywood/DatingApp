using DatingApp.Backend.Application.Contracts.Persistence.Repositories;
using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.Helpers;
using DatingApp.Backend.Application.Helpers.Params;
using DatingApp.Backend.Domain.Entities;
using DatingApp.Backend.Domain.Extensions;

namespace DatingApp.Backend.Infrastructure.Persistence.Repositories;

public class LikesRepository(DatingAppDbContext context) : AsyncRepository<UserLike>(context), ILikesRepository
{
    public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserId) =>
        await Context.Likes.FindAsync(sourceUserId, targetUserId);

    public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
    {
        var users = Context.Users.Where(u => u.Id < 0);
        var likes = Context.Likes.AsQueryable();

        switch (likesParams.Predicate)
        {
            case "liked":
                likes = likes.Where(l => l.SourceUserId == likesParams.UserId);
                users = likes.Select(l => l.TargetUser);
                break;
            case "likedBy":
                likes = likes.Where(l => l.TargetUserId == likesParams.UserId);
                users = likes.Select(l => l.SourceUser);
                break;
        }

        var likedUsers = users.OrderBy(u => u.Username).Select(user => new LikeDto
        {
            Id = user.Id,
            Username = user.Username,
            Age = user.DateOfBirth.CalculateAge(),
            KnownAs = user.KnownAs,
            City = user.City,
            PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
        });

        return await PagedList<LikeDto>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);
    }
}