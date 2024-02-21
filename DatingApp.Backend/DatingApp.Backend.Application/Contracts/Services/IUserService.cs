using DatingApp.Backend.Application.DTOs;

namespace DatingApp.Backend.Application.Contracts.Services;

public interface IUserService
{
    Task<MemberDto> GetUserByIdAsync(int id);
    Task<MemberDto> GetUserByUsernameAsync(string username);
    Task<IEnumerable<MemberDto>> ListAllUsersAsync();
    Task UpdateUserAsync(string username, MemberUpdateDto memberUpdateDto);
    Task<bool> UserExistsAsync(string username);
}
