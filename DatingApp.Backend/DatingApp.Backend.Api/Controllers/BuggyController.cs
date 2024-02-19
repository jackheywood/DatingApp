using DatingApp.Backend.Application.Contracts.Services;
using DatingApp.Backend.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Backend.Api.Controllers;

public class BuggyController(IUserService userService) : BaseApiController
{
    [Authorize]
    [HttpGet("auth")]
    public ActionResult<string> GetSecret()
    {
        return "secret text";
    }

    [HttpGet("not-found")]
    public async Task<ActionResult<MemberDto>> GetNotFound()
    {
        var user = await userService.GetUserByIdAsync(-1);

        if (user is null) return NotFound();

        return user;
    }

    [HttpGet("server-error")]
    public async Task<ActionResult<string>> GetServerError()
    {
        var user = await userService.GetUserByIdAsync(-1);

        return user.ToString();
    }

    [HttpGet("bad-request")]
    public ActionResult GetBadRequest()
    {
        return BadRequest("This was not a good request");
    }
}
