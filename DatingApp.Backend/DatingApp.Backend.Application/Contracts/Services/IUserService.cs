using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.Helpers;
using DatingApp.Backend.Application.Helpers.Params;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Backend.Application.Contracts.Services;

public interface IUserService
{
    Task<MemberDto> GetUserByIdAsync(int id);
    Task<MemberDto> GetUserByUsernameAsync(string username);
    Task<PagedList<MemberDto>> ListUsersAsync(string username, UserParams userParams);
    Task UpdateUserAsync(string username, MemberUpdateDto memberUpdateDto);
    Task LogUserActivity(int userId);
    Task<PhotoDto> AddPhotoAsync(string username, IFormFile file);
    Task SetMainPhotoAsync(string username, int photoId);
    Task DeletePhotoAsync(string username, int photoId);
    Task<bool> UserExistsAsync(string username);
}