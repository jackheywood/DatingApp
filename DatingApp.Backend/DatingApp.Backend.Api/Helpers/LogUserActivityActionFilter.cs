using DatingApp.Backend.Api.Extensions;
using DatingApp.Backend.Application.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DatingApp.Backend.Api.Helpers;

public class LogUserActivityActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var resultContext = await next();

        if (!IsAuthenticated(resultContext)) return;
        
        var userService = resultContext.HttpContext.RequestServices.GetRequiredService<IUserService>();
        
        await userService.LogUserActivity(resultContext.HttpContext.User.GetUserId());
    }

    private static bool IsAuthenticated(ActionContext context)
    {
        return context.HttpContext.User.Identity?.IsAuthenticated ?? false;
    }
}