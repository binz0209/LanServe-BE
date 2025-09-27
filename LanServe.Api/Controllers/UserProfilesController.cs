using LanServe.Application.Interfaces.Services;
using LanServe.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanServe.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserProfilesController : ControllerBase
{
    private readonly IUserProfileService _svc;
    public UserProfilesController(IUserProfileService svc) { _svc = svc; }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id) => Ok(await _svc.GetByIdAsync(id));

    [Authorize]
    [HttpGet("by-user/{userId}")]
    public async Task<IActionResult> GetByUser(string userId) => Ok(await _svc.GetByUserIdAsync(userId));

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserProfile dto) => Ok(await _svc.CreateAsync(dto));

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UserProfile dto) => Ok(await _svc.UpdateAsync(id, dto));

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id) => Ok(await _svc.DeleteAsync(id));
}
