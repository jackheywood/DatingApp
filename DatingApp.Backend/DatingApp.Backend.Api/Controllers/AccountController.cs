using DatingApp.Backend.Application.Contracts.Identity;
using DatingApp.Backend.Application.Contracts.Services;
using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.DTOs.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Backend.Api.Controllers;

public class AccountController(IIdentityService identityService, IUserService userService) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        if (await userService.UserExistsAsync(registerDto.Username)) return BadRequest("Username is taken");

        var user = await identityService.RegisterUserAsync(registerDto);

        return CreatedAtAction(nameof(UsersController.GetUser), "Users", new { id = user.Id }, null);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        if (!await userService.UserExistsAsync(loginDto.Username)) return Unauthorized("Invalid username");

        var user = await identityService.AuthenticateUserAsync(loginDto);

        return user is null ? Unauthorized("Invalid password") : Ok(user);
    }
}
