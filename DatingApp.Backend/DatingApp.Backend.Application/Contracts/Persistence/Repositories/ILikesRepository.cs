using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Domain.Entities;

namespace DatingApp.Backend.Application.Contracts.Persistence.Repositories;

public interface ILikesRepository : IAsyncRepository<UserLike>
{
    Task<UserLike> GetUserLike(int sourceUserId, int targetUserId);
    Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId);
}