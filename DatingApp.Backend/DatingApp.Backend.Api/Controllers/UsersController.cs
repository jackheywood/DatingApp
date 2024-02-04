﻿using DatingApp.Backend.Application.Contracts.Services;
using DatingApp.Backend.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        return Ok(await userService.ListAllUsersAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        return Ok(await userService.GetUserByIdAsync(id));
    }
}