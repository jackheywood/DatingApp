using AutoMapper;
using DatingApp.Backend.Application.Contracts.Persistence.Repositories;
using DatingApp.Backend.Application.Contracts.Services;
using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.Exceptions;
using DatingApp.Backend.Application.Extensions;
using DatingApp.Backend.Domain.Entities;

namespace DatingApp.Backend.Application.Services;

public class MessageService(IMessageRepository messageRepository, IUserRepository userRepository, IMapper mapper)
    : IMessageService
{
    public async Task<MessageDto> CreateMessageAsync(string senderUsername, CreateMessageDto createMessageDto)
    {
        if (senderUsername.EqualsIgnoreCase(createMessageDto.RecipientUsername))
            throw new MessageException("You cannot send messages to yourself");

        var sender = await userRepository.GetByUsernameAsync(senderUsername);
        if (sender is null) throw new NotFoundException($"User {senderUsername} not found");

        var recipient = await userRepository.GetByUsernameAsync(createMessageDto.RecipientUsername);
        if (recipient is null) throw new NotFoundException($"User {createMessageDto.RecipientUsername} not found");

        var message = new Message
        {
            Content = createMessageDto.Content,
            Sender = sender,
            SenderUsername = sender.Username,
            Recipient = recipient,
            RecipientUsername = recipient.Username,
        };

        messageRepository.AddMessage(message);
        if (await messageRepository.SaveAllAsync()) return mapper.Map<MessageDto>(message);

        throw new MessageException($"Failed to send message to {createMessageDto.RecipientUsername}");
    }
}