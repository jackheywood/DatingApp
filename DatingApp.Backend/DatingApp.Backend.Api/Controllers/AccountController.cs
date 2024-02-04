using DatingApp.Backend.Application.Contracts.Identity;
using DatingApp.Backend.Application.Contracts.Services;
using DatingApp.Backend.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Backend.Api.Controllers;

public class AccountController(IIdentityService identityService, IUserService userService) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        if (await userService.UserExistsAsync(registerDto.UserName)) return BadRequest("Username is taken");

        var user = await identityService.RegisterUserAsync(registerDto);

        return CreatedAtAction(nameof(UsersController.GetUser), "Users", new { id = user.Id }, null);
    }
}
