using DatingApp.Backend.Application.Contracts.Persistence.Repositories;

namespace DatingApp.Backend.Infrastructure.Persistence.Repositories;

public abstract class AsyncRepository<T>(DatingAppDbContext context) : IAsyncRepository<T> where T : class
{
    protected DatingAppDbContext Context = context;

    public async Task AddAsync(T entity)
    {
        await Context.AddAsync(entity);
    }

    public async Task<bool> SaveAllAsync() => await Context.SaveChangesAsync() > 0;
}