using System.Net;
using System.Text.Json;
using DatingApp.Backend.Api.Exceptions;
using DatingApp.Backend.Application.Exceptions;

namespace DatingApp.Backend.Api.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException ex)
        {
            logger.LogError(ex, "{Message}", ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            var response = JsonSerializer.Serialize(ex.Message);
            await context.Response.WriteAsync(response);
        }
        catch (Exception ex)
            when (ex is UpdateFailedException or PhotoUploadException or LikeException or MessageException)
        {
            logger.LogError(ex, "{Message}", ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var response = JsonSerializer.Serialize(ex.Message);
            await context.Response.WriteAsync(response);
        }
        catch (MessageAuthorizationException ex)
        {
            logger.LogError(ex, "{Message}", ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            var response = JsonSerializer.Serialize(ex.Message);
            await context.Response.WriteAsync(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Message}", ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = env.IsDevelopment()
                ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace)
                : new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error");

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }
}