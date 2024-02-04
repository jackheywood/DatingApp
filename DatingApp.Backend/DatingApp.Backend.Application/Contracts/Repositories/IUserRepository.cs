using DatingApp.Backend.Domain.Entities;

namespace DatingApp.Backend.Application.Contracts.Repositories;

public interface IUserRepository : IAsyncRepository<AppUser>
{
    Task<AppUser> GetByUserNameAsync(string userName);
    Task<bool> ExistsAsync(string userName);
}
