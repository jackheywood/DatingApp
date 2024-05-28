using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.Backend.Application.Contracts.Persistence.Repositories;
using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.Helpers;
using DatingApp.Backend.Application.Helpers.Params;
using DatingApp.Backend.Domain.Entities;

namespace DatingApp.Backend.Infrastructure.Persistence.Repositories;

public class MessageRepository(DatingAppDbContext context, IMapper mapper)
    : AsyncRepository<Message>(context), IMessageRepository
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

    public async Task<PagedList<MessageDto>> GetMessagesForUserAsync(MessageParams messageParams)
    {
        var query = Context.Messages
            .OrderByDescending(m => m.DateSent)
            .AsQueryable();

        query = messageParams.Container switch
        {
            "Inbox" => query.Where(m => m.RecipientUsername == messageParams.Username),
            "Outbox" => query.Where(m => m.SenderUsername == messageParams.Username),
            _ => query.Where(m => m.RecipientUsername == messageParams.Username && m.DateRead == null),
        };

        var messages = query.ProjectTo<MessageDto>(mapper.ConfigurationProvider);

        return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
    }

    public async Task<IEnumerable<MessageDto>> GetMessageThreadAsync(int userId, int recipientId) =>
        throw new NotImplementedException();
}