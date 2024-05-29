using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.Helpers;
using DatingApp.Backend.Application.Helpers.Params;
using DatingApp.Backend.Domain.Entities;

namespace DatingApp.Backend.Application.Contracts.Persistence.Repositories;

public interface ILikesRepository : IAsyncRepository<UserLike>
{
    Task<UserLike> GetUserLike(int sourceUserId, int targetUserId);
    Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
}