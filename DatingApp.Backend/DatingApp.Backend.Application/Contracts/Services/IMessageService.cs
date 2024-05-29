using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.Helpers;
using DatingApp.Backend.Application.Helpers.Params;

namespace DatingApp.Backend.Application.Contracts.Services;

public interface IMessageService
{
    Task<PagedList<MessageDto>> GetMessagesForUserAsync(MessageParams messageParams);
    Task<IEnumerable<MessageDto>> GetMessageThreadAsync(string currentUsername, string recipientUsername);
    Task<MessageDto> CreateMessageAsync(string senderUsername, CreateMessageDto createMessageDto);
    Task DeleteMessageAsync(int id, string username);
}