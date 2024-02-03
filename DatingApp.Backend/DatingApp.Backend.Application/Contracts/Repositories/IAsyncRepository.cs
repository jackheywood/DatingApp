namespace DatingApp.Backend.Application.Contracts.Repositories;

public interface IAsyncRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> ListAllAsync();
}
