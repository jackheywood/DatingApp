using DatingApp.Backend.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Backend.Application.Contracts.Services;

public interface IUserService
{
    Task<MemberDto> GetUserByIdAsync(int id);
    Task<MemberDto> GetUserByUsernameAsync(string username);
    Task<IEnumerable<MemberDto>> ListAllUsersAsync();
    Task UpdateUserAsync(string username, MemberUpdateDto memberUpdateDto);
    Task<PhotoDto> AddPhotoAsync(string username, IFormFile file);
    Task<bool> UserExistsAsync(string username);
}
