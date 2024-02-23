using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Domain.Entities;

namespace DatingApp.Backend.Application.Contracts.Persistence.Repositories;

public interface IUserRepository : IAsyncRepository<AppUser>
{
    Task<AppUser> GetByUsernameAsync(string username);
    Task<bool> ExistsAsync(string username);
    Task<IEnumerable<MemberDto>> GetMembersAsync();
    Task<MemberDto> GetMemberAsync(string username);
}
