using DatingApp.Backend.Application.Contracts.Persistence.Repositories;
using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.Helpers;
using DatingApp.Backend.Domain.Entities;

namespace DatingApp.Backend.Infrastructure.Persistence.Repositories;

public class MessageRepository(DatingAppDbContext context) : AsyncRepository<Message>(context), IMessageRepository
{
    public void AddMessage(Message message)
    {
        Context.Messages.Add(message);
    }

    public void DeleteMessage(Message message)
    {
        Context.Messages.Remove(message);
    }

    public async Task<Message> GetMessageAsync(int id) => await Context.Messages.FindAsync(id);

    public async Task<PagedList<MessageDto>> GetMessagesForUserAsync(int userId) => throw new NotImplementedException();

    public async Task<IEnumerable<MessageDto>> GetMessageThreadAsync(int userId, int recipientId) =>
        throw new NotImplementedException();
}