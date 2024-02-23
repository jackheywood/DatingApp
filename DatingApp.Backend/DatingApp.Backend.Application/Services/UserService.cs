using AutoMapper;
using DatingApp.Backend.Application.Contracts.Persistence.Repositories;
using DatingApp.Backend.Application.Contracts.Persistence.Services;
using DatingApp.Backend.Application.Contracts.Services;
using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.Exceptions;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Backend.Application.Services;

public class UserService(IUserRepository userRepository, IPhotoService photoService, IMapper mapper) : IUserService
{
    public async Task<MemberDto> GetUserByIdAsync(int id)
    {
        var user = await userRepository.GetByIdAsync(id);
        return mapper.Map<MemberDto>(user);
    }

    public async Task<MemberDto> GetUserByUsernameAsync(string username)
    {
        return await userRepository.GetMemberAsync(username);
    }

    public async Task<IEnumerable<MemberDto>> ListAllUsersAsync()
    {
        return await userRepository.GetMembersAsync();
    }

    public async Task UpdateUserAsync(string username, MemberUpdateDto memberUpdateDto)
    {
        var user = await userRepository.GetByUsernameAsync(username);

        if (user is null) throw new NotFoundException($"User {username} not found");

        mapper.Map(memberUpdateDto, user);

        var saveResult = await userRepository.SaveAllAsync();
        if (!saveResult) throw new UpdateFailedException($"Failed to update user {username}");
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

    public async Task<bool> UserExistsAsync(string username)
    {
        return await userRepository.ExistsAsync(username);
    }
}
