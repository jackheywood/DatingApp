using DatingApp.Backend.Api.Extensions;
using DatingApp.Backend.Application.Contracts.Services;
using DatingApp.Backend.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Backend.Api.Controllers;

public class MessagesController(IMessageService messageService) : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
    {
        var username = User.GetUsername();
        var message = await messageService.CreateMessageAsync(username, createMessageDto);
        return Ok(message);
    }
}