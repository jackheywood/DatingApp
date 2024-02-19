using AutoMapper;
using DatingApp.Backend.Application.Contracts.Repositories;
using DatingApp.Backend.Application.Contracts.Services;
using DatingApp.Backend.Application.DTOs;

namespace DatingApp.Backend.Application.Services;

public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
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

    public async Task<bool> UserExistsAsync(string username)
    {
        return await userRepository.ExistsAsync(username);
    }
}
