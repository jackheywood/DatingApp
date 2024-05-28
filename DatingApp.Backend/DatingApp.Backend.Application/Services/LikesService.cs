using DatingApp.Backend.Application.Contracts.Persistence.Repositories;
using DatingApp.Backend.Application.Contracts.Services;
using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.Exceptions;
using DatingApp.Backend.Application.Helpers;
using DatingApp.Backend.Domain.Entities;

namespace DatingApp.Backend.Application.Services;

public class LikesService(ILikesRepository likesRepository, IUserRepository userRepository) : ILikesService
{
    public async Task<PagedList<LikeDto>> GetUserLikesAsync(LikesParams likesParams) =>
        await likesRepository.GetUserLikes(likesParams);

    public async Task AddLikeAsync(string username, int sourceUserId)
    {
        var likedUser = await userRepository.GetByUsernameAsync(username);
        if (likedUser is null) throw new NotFoundException($"User {username} not found");

        var sourceUser = await userRepository.GetUserWithLikes(sourceUserId);
        if (sourceUser is null) throw new NotFoundException($"User {sourceUserId} not found");
        if (sourceUser.Username == username) throw new LikeException("You cannot like yourself");

        var userLike = await likesRepository.GetUserLike(sourceUserId, likedUser.Id);
        if (userLike is not null) throw new LikeException("You already like this user");

        sourceUser.LikedUsers.Add(new UserLike
        {
            SourceUserId = sourceUserId,
            TargetUserId = likedUser.Id,
        });

        if (await userRepository.SaveAllAsync()) return;
        throw new LikeException("Failed to like user");
    }
}