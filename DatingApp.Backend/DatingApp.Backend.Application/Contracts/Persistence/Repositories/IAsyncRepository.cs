namespace DatingApp.Backend.Application.Contracts.Persistence.Repositories;

public interface IAsyncRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    void Update(T entity);
    Task AddAsync(T entity);
    Task<bool> SaveAllAsync();
}
