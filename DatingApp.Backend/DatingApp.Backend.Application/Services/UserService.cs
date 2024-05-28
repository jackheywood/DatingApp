using AutoMapper;
using DatingApp.Backend.Application.Contracts.Persistence.Repositories;
using DatingApp.Backend.Application.Contracts.Persistence.Services;
using DatingApp.Backend.Application.Contracts.Services;
using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.Exceptions;
using DatingApp.Backend.Application.Helpers;
using DatingApp.Backend.Application.Helpers.Params;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Backend.Application.Services;

public class UserService(IUserRepository userRepository, IPhotoService photoService, IMapper mapper) : IUserService
{
    public async Task<MemberDto> GetUserByIdAsync(int id)
    {
        var user = await userRepository.GetByIdAsync(id);
        return mapper.Map<MemberDto>(user);
    }

    public async Task<MemberDto> GetUserByUsernameAsync(string username) =>
        await userRepository.GetMemberAsync(username);

    public async Task<PagedList<MemberDto>> ListUsersAsync(string username, UserParams userParams)
    {
        var currentUser = await userRepository.GetByUsernameAsync(username);
        userParams.CurrentUsername = currentUser.Username;

        if (string.IsNullOrEmpty(userParams.Gender))
            userParams.Gender = currentUser.Gender == "male" ? "female" : "male";

        return await userRepository.GetMembersAsync(userParams);
    }

    public async Task UpdateUserAsync(string username, MemberUpdateDto memberUpdateDto)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        if (user is null) throw new NotFoundException($"User {username} not found");

        mapper.Map(memberUpdateDto, user);

        var saveResult = await userRepository.SaveAllAsync();
        if (!saveResult) throw new UpdateFailedException($"Failed to update user {username}");
    }

    public async Task LogUserActivity(int userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        user.LastActive = DateTime.UtcNow;
        await userRepository.SaveAllAsync();
    }

    public async Task<PhotoDto> AddPhotoAsync(string username, IFormFile file)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        if (user is null) throw new NotFoundException($"User {username} not found");

        var photo = await photoService.AddPhotoAsync(file);
        if (user.Photos.Count == 0) photo.IsMain = true;

        user.Photos.Add(photo);

        var saveResult = await userRepository.SaveAllAsync();

        if (!saveResult) throw new UpdateFailedException($"Failed to update user {username}");

        return mapper.Map<PhotoDto>(photo);
    }

    public async Task SetMainPhotoAsync(string username, int photoId)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        if (user is null) throw new NotFoundException($"User {username} not found");

        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
        if (photo is null) throw new NotFoundException($"Photo {photoId} does not belong to {username}");
        if (photo.IsMain) throw new UpdateFailedException($"Photo {photoId} is already the main photo for {username}");

        var currentMain = user.Photos.FirstOrDefault(p => p.IsMain);
        if (currentMain != null) currentMain.IsMain = false;

        photo.IsMain = true;

        var saveResult = await userRepository.SaveAllAsync();
        if (!saveResult) throw new UpdateFailedException($"Failed to update main photo for {username}");
    }

    public async Task DeletePhotoAsync(string username, int photoId)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        if (user is null) throw new NotFoundException($"User {username} not found");

        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
        if (photo is null) throw new NotFoundException($"Photo {photoId} does not belong to {username}");
        if (photo.IsMain) throw new UpdateFailedException("You cannot delete your main photo");

        if (photo.PublicId is not null) await photoService.DeletePhotoAsync(photo.PublicId);

        user.Photos.Remove(photo);

        var saveResult = await userRepository.SaveAllAsync();
        if (!saveResult) throw new UpdateFailedException($"Failed to delete photo for {username}");
    }

    public async Task<bool> UserExistsAsync(string username) => await userRepository.ExistsAsync(username);
}