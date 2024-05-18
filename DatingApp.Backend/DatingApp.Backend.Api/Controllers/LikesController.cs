using DatingApp.Backend.Api.Extensions;
using DatingApp.Backend.Application.Contracts.Services;
using DatingApp.Backend.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Backend.Api.Controllers;

public class LikesController(ILikesService likesService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes(string predicate)
    {
        var likes = await likesService.GetUserLikesAsync(predicate, User.GetUserId());
        return Ok(likes);
    }

    [HttpPost("{username}")]
    public async Task<ActionResult> AddLike(string username)
    {
        await likesService.AddLikeAsync(username, User.GetUserId());
        return NoContent();
    }
}