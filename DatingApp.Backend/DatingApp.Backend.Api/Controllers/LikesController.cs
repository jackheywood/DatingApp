using DatingApp.Backend.Api.Extensions;
using DatingApp.Backend.Api.Helpers;
using DatingApp.Backend.Application.Contracts.Services;
using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.Helpers;
using DatingApp.Backend.Application.Helpers.Params;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Backend.Api.Controllers;

public class LikesController(ILikesService likesService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes([FromQuery] LikesParams likesParams)
    {
        likesParams.UserId = User.GetUserId();
        var likes = await likesService.GetUserLikesAsync(likesParams);
        Response.AddPaginationHeader(new PaginationHeader(likes));
        return Ok(likes);
    }

    [HttpPost("{username}")]
    public async Task<ActionResult> AddLike(string username)
    {
        await likesService.AddLikeAsync(username, User.GetUserId());
        return NoContent();
    }
}