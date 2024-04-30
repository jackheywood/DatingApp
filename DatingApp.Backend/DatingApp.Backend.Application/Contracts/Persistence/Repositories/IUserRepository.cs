using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.Helpers;
using DatingApp.Backend.Domain.Entities;

namespace DatingApp.Backend.Application.Contracts.Persistence.Repositories;

public interface IUserRepository : IAsyncRepository<AppUser>
{
    Task<AppUser> GetByUsernameAsync(string username);
    Task<bool> ExistsAsync(string username);
    Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
    Task<MemberDto> GetMemberAsync(string username);
}