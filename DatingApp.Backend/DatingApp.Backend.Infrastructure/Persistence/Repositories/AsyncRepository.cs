using DatingApp.Backend.Application.Contracts.Persistence.Repositories;
using DatingApp.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Backend.Infrastructure.Persistence.Repositories;

public abstract class AsyncRepository<T>(DatingAppDbContext context) : IAsyncRepository<T> where T : class
{
    protected DatingAppDbContext Context = context;
    
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Context.Set<T>().ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await Context.AddAsync(entity);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await Context.SaveChangesAsync() > 0;
    }

    public void Update(T entity)
    {
        Context.Entry(entity).State = EntityState.Modified;
    }
}