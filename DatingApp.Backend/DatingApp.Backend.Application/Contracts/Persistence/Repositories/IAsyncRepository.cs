namespace DatingApp.Backend.Application.Contracts.Persistence.Repositories;

public interface IAsyncRepository<in T> where T : class
{
    Task AddAsync(T entity);
    Task<bool> SaveAllAsync();
}
