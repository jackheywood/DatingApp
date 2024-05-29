using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.Backend.Application.Contracts.Persistence.Repositories;
using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.Helpers;
using DatingApp.Backend.Application.Helpers.Params;
using DatingApp.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
            "Inbox" => query.Where(IsInboxMessage(messageParams.Username)),
            "Outbox" => query.Where(IsOutboxMessage(messageParams.Username)),
            _ => query.Where(IsUnreadMessage(messageParams.Username)),
        };

        var messages = query.ProjectTo<MessageDto>(mapper.ConfigurationProvider);

        return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
    }

    public async Task<IEnumerable<MessageDto>> GetMessageThreadAsync(string currentUsername, string recipientUsername)
    {
        var messages = await Context.Messages
            .Include(m => m.Sender).ThenInclude(s => s.Photos)
            .Include(m => m.Recipient).ThenInclude(r => r.Photos)
            .Where(IsMessageBetweenUsers(currentUsername, recipientUsername))
            .OrderBy(m => m.DateSent)
            .ToListAsync();

        await MarkUnreadMessagesAsReadAsync(messages, currentUsername);

        return mapper.Map<IEnumerable<MessageDto>>(messages);
    }

    private async Task MarkUnreadMessagesAsReadAsync(IEnumerable<Message> messages, string username)
    {
        var unreadMessages = messages.Where(IsUnreadByUser(username)).ToList();

        if (unreadMessages.Count != 0)
        {
            foreach (var message in unreadMessages)
            {
                message.DateRead = DateTime.UtcNow;
            }

            await Context.SaveChangesAsync();
        }
    }

    private static Expression<Func<Message, bool>> IsInboxMessage(string username)
    {
        return m => m.RecipientUsername == username && !m.RecipientDeleted;
    }

    private static Expression<Func<Message, bool>> IsOutboxMessage(string username)
    {
        return m => m.SenderUsername == username && !m.SenderDeleted;
    }

    private static Expression<Func<Message, bool>> IsUnreadMessage(string username)
    {
        return m => m.RecipientUsername == username && m.DateRead == null && !m.RecipientDeleted;
    }

    private static Expression<Func<Message, bool>> IsMessageBetweenUsers(string username1, string username2)
    {
        return m => (m.RecipientUsername == username1 && !m.RecipientDeleted && m.SenderUsername == username2)
                    || (m.SenderUsername == username1 && !m.SenderDeleted && m.RecipientUsername == username2);
    }

    private static Func<Message, bool> IsUnreadByUser(string username)
    {
        return m => m.RecipientUsername == username && m.DateRead == null;
    }
}