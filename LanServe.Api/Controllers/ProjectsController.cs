using LanServe.Application.Interfaces.Services;
using LanServe.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanServe.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _svc;
    public ProjectsController(IProjectService svc) { _svc = svc; }

    [AllowAnonymous]
    [HttpGet("{id}")] public async Task<IActionResult> GetById(string id) => Ok(await _svc.GetByIdAsync(id));

    [AllowAnonymous]
    [HttpGet("open")] public async Task<IActionResult> Open() => Ok(await _svc.GetOpenProjectsAsync());

    [Authorize]
    [HttpGet("by-owner/{ownerId}")]
    public async Task<IActionResult> ByOwner(string ownerId) => Ok(await _svc.GetByOwnerIdAsync(ownerId));

    [Authorize(Roles = "User,Admin")]
    [HttpPost] public async Task<IActionResult> Create([FromBody] Project dto) => Ok(await _svc.CreateAsync(dto));

    [Authorize(Roles = "User,Admin")]
    [HttpPut("{id}")] public async Task<IActionResult> Update(string id, [FromBody] Project dto) => Ok(await _svc.UpdateAsync(id, dto));

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")] public async Task<IActionResult> Delete(string id) => Ok(await _svc.DeleteAsync(id));
}
