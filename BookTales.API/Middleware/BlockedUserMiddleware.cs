using System.Security.Claims;
using BookTales.Application.Interfaces.Repositories;

namespace BookTales.API.Middleware;

public class BlockedUserMiddleware
{
    private readonly RequestDelegate _next;

    public BlockedUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        IUserRepository userRepository)
    {
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            var userIdClaim =
                context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdClaim, out var userId))
            {
                var user = await userRepository.GetByIdAsync(userId);

                if (user != null && user.IsBlocked)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(
                        "{\"success\":false,\"message\":\"Your account has been blocked.\"}");

                    return;
                }
            }
        }

        await _next(context);
    }
}