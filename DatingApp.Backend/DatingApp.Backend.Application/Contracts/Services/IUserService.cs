using DatingApp.Backend.Domain.Entities;

namespace DatingApp.Backend.Application.Contracts.Services;

public interface IUserService
{
    Task<AppUser> GetUserByIdAsync(int id);
    Task<IEnumerable<AppUser>> ListAllUsersAsync();
}
