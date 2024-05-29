using DatingApp.Backend.Api.Extensions;
using DatingApp.Backend.Api.Helpers;
using DatingApp.Backend.Application.Contracts.Services;
using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.Helpers;
using DatingApp.Backend.Application.Helpers.Params;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Backend.Api.Controllers;

public class MessagesController(IMessageService messageService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
    {
        messageParams.Username = User.GetUsername();
        var messages = await messageService.GetMessagesForUserAsync(messageParams);
        Response.AddPaginationHeader(new PaginationHeader(messages));
        return Ok(messages);
    }

    [HttpGet("thread/{username}")]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
    {
        var currentUsername = User.GetUsername();
        var thread = await messageService.GetMessageThreadAsync(currentUsername, username);
        return Ok(thread);
    }

    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
    {
        var username = User.GetUsername();
        var message = await messageService.CreateMessageAsync(username, createMessageDto);
        return Ok(message);
    }
}