using DatingApp.Backend.Application.DTOs;

namespace DatingApp.Backend.Application.Contracts.Services;

public interface ILikesService
{
    Task<IEnumerable<LikeDto>> GetUserLikesAsync(string predicate, int userId);
    Task AddLikeAsync(string username, int sourceUserId);
}