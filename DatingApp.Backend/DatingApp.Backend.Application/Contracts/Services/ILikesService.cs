using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.Helpers;
using DatingApp.Backend.Application.Helpers.Params;

namespace DatingApp.Backend.Application.Contracts.Services;

public interface ILikesService
{
    Task<PagedList<LikeDto>> GetUserLikesAsync(LikesParams likesParams);
    Task AddLikeAsync(string username, int sourceUserId);
}