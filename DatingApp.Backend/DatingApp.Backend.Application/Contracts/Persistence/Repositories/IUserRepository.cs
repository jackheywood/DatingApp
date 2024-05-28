using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.Helpers;
using DatingApp.Backend.Application.Helpers.Params;
using DatingApp.Backend.Domain.Entities;

namespace DatingApp.Backend.Application.Contracts.Persistence.Repositories;

public interface IUserRepository : IAsyncRepository<AppUser>
{
    Task<AppUser> GetByIdAsync(int userId);
    Task<AppUser> GetByUsernameAsync(string username);
    Task<AppUser> GetUserWithLikes(int userId);
    Task<bool> ExistsAsync(string username);
    Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
    Task<MemberDto> GetMemberAsync(string username);
}