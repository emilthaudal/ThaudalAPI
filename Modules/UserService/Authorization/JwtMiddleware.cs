using Microsoft.AspNetCore.Http;
using UserService.Interfaces;

namespace UserService.Authorization;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserService userService, IJwtService jwtService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = await jwtService.ValidateJwtToken(token);
        if (userId != default) context.Items["User"] = userService.GetById(userId);

        await _next(context);
    }
}