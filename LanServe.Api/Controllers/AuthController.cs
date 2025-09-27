using LanServe.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanServe.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _users;
    private readonly IJwtTokenService _jwt;
    public AuthController(IUserService users, IJwtTokenService jwt)
    {
        _users = users; _jwt = jwt;
    }

    public record RegisterRequest(string FullName, string Email, string Password, string Role = "User");
    public record LoginRequest(string Email, string Password);

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        var user = await _users.RegisterAsync(req.FullName, req.Email, req.Password, req.Role);
        var (token, exp) = _jwt.GenerateToken(user.Id, user.Email, user.Role);
        return Ok(new { accessToken = token, expiresIn = exp });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var user = await _users.ValidateUserAsync(req.Email, req.Password);
        if (user is null) return Unauthorized(new { message = "Invalid credentials" });
        var (token, exp) = _jwt.GenerateToken(user.Id, user.Email, user.Role);
        return Ok(new { accessToken = token, expiresIn = exp });
    }
}
