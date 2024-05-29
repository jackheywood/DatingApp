using AutoMapper;
using DatingApp.Backend.Application.Contracts.Persistence.Repositories;
using DatingApp.Backend.Application.Contracts.Services;
using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.Exceptions;
using DatingApp.Backend.Application.Extensions;
using DatingApp.Backend.Application.Helpers;
using DatingApp.Backend.Application.Helpers.Params;
using DatingApp.Backend.Domain.Entities;

namespace DatingApp.Backend.Application.Services;

public class MessageService(IMessageRepository messageRepository, IUserRepository userRepository, IMapper mapper)
    : IMessageService
{
    public async Task<PagedList<MessageDto>> GetMessagesForUserAsync(MessageParams messageParams)
    {
        var userExists = await userRepository.ExistsAsync(messageParams.Username);
        if (!userExists) throw new NotFoundException($"User {messageParams.Username} not found");

        return await messageRepository.GetMessagesForUserAsync(messageParams);
    }

    public async Task<IEnumerable<MessageDto>> GetMessageThreadAsync(string currentUsername, string recipientUsername)
    {
        if (currentUsername.EqualsIgnoreCase(recipientUsername))
            throw new MessageException("You cannot read messages from yourself");
        
        var currentUserExists = await userRepository.ExistsAsync(currentUsername);
        if (!currentUserExists) throw new NotFoundException($"User {currentUsername} not found");

        var recipientExists = await userRepository.ExistsAsync(recipientUsername);
        if (!recipientExists) throw new NotFoundException($"User {recipientUsername} not found");

        return await messageRepository.GetMessageThreadAsync(currentUsername, recipientUsername);
    }

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