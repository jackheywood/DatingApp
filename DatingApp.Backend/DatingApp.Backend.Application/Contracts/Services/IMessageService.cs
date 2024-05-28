using DatingApp.Backend.Application.DTOs;

namespace DatingApp.Backend.Application.Contracts.Services;

public interface IMessageService
{
    Task<MessageDto> CreateMessageAsync(string senderUsername, CreateMessageDto createMessageDto);
}