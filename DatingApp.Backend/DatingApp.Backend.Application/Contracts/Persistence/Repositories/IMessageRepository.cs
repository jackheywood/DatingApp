using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.Helpers;
using DatingApp.Backend.Domain.Entities;

namespace DatingApp.Backend.Application.Contracts.Persistence.Repositories;

public interface IMessageRepository : IAsyncRepository<Message>
{
    void AddMessage(Message message);
    void DeleteMessage(Message message);
    Task<Message> GetMessageAsync(int id);
    Task<PagedList<MessageDto>> GetMessagesForUserAsync(int userId);
    Task<IEnumerable<MessageDto>> GetMessageThreadAsync(int userId, int recipientId);
}