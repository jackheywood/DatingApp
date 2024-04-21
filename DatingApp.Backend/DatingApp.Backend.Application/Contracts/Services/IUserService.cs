﻿using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Backend.Application.Contracts.Services;

public interface IUserService
{
    Task<MemberDto> GetUserByIdAsync(int id);
    Task<MemberDto> GetUserByUsernameAsync(string username);
    Task<PagedList<MemberDto>> ListUsersAsync(UserParams userParams);
    Task UpdateUserAsync(string username, MemberUpdateDto memberUpdateDto);
    Task<PhotoDto> AddPhotoAsync(string username, IFormFile file);
    Task SetMainPhotoAsync(string username, int photoId);
    Task DeletePhotoAsync(string username, int photoId);
    Task<bool> UserExistsAsync(string username);
}