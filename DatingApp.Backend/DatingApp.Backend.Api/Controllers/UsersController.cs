using DatingApp.Backend.Api.Extensions;
using DatingApp.Backend.Api.Helpers;
using DatingApp.Backend.Application.Contracts.Services;
using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Backend.Api.Controllers;

[Authorize]
public class UsersController(IUserService userService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
    {
        var users = await userService.ListUsersAsync(userParams);

        Response.AddPaginationHeader(new PaginationHeader(users));

        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MemberDto>> GetUserById(int id)
    {
        return Ok(await userService.GetUserByIdAsync(id));
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUserByUsername(string username)
    {
        return Ok(await userService.GetUserByUsernameAsync(username));
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        await userService.UpdateUserAsync(User.GetUsername(), memberUpdateDto);
        return NoContent();
    }

    [HttpPost("photos")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var username = User.GetUsername();
        var photo = await userService.AddPhotoAsync(username, file);
        return CreatedAtAction(nameof(GetUserByUsername), new { username }, photo);
    }

    [HttpPut("photos/set-main/{photoId:int}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        await userService.SetMainPhotoAsync(User.GetUsername(), photoId);
        return NoContent();
    }

    [HttpDelete("photos/{photoId:int}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        await userService.DeletePhotoAsync(User.GetUsername(), photoId);
        return NoContent();
    }
}