﻿using System.Security.Claims;

namespace DatingApp.Backend.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        return Convert.ToInt32(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    }
    
    public static string GetUsername(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Name)?.Value;
    }
}
