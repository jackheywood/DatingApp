using DatingApp.Backend.Application.Contracts.Repositories;
using DatingApp.Backend.Application.Contracts.Services;
using DatingApp.Backend.Domain.Entities;

namespace DatingApp.Backend.Application.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<AppUser> GetUserByIdAsync(int id)
    {
        return await userRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<AppUser>> ListAllUsersAsync()
    {
        return await userRepository.ListAllAsync();
    }

    public async Task<bool> UserExistsAsync(string userName)
    {
        return await userRepository.ExistsAsync(userName);
    }
}
