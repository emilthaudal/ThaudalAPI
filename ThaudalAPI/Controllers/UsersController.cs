﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Interfaces;
using UserService.Model.Auth;
using UserService.Model.Users;

namespace ThaudalAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserRequest userRequest)
    {
        var response = await _userService.CreateUser(userRequest);
        if (response.Result != CreateUserResult.Ok || response.User == null) return BadRequest(response.Result);
        var authResponse = await _userService.Authenticate(new AuthenticateRequest
        {
            Password = userRequest.Password,
            Username = response.User?.Username
        }, IpAddress());
        SetTokenCookie(authResponse.RefreshToken);
        return Ok(response.User);
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
    {
        var response = await _userService.Authenticate(model, IpAddress());
        SetTokenCookie(response.RefreshToken);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken)) return BadRequest();
        var response = await _userService.RefreshToken(refreshToken, IpAddress());
        SetTokenCookie(response.RefreshToken);
        return Ok(response);
    }

    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken(RevokeTokenRequest model)
    {
        var token = model.Token;

        if (string.IsNullOrEmpty(token))
            return BadRequest(new {message = "Token is required"});

        await _userService.RevokeToken(token, IpAddress());
        return Ok(new {message = "Token revoked"});
    }

    [Authorize(Roles = "Administrator")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAll();
        return Ok(users);
    }

    [Authorize(Roles = "Administrator")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.GetById(id);
        return Ok(user);
    }

    [Authorize(Roles = "Administrator")]
    [HttpGet("{id:guid}/refresh-tokens")]
    public async Task<IActionResult> GetRefreshTokens([FromQuery] Guid id)
    {
        var user = await _userService.GetById(id);
        return Ok(user.RefreshTokens);
    }

    private void SetTokenCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }

    private string IpAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"];
        return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
    }
}