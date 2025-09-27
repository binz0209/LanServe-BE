using LanServe.Application.Interfaces.Services;
using LanServe.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanServe.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectSkillsController : ControllerBase
{
    private readonly IProjectSkillService _svc;
    public ProjectSkillsController(IProjectSkillService svc) { _svc = svc; }

    [AllowAnonymous]
    [HttpGet("by-project/{projectId}")]
    public async Task<IActionResult> ByProject(string projectId) => Ok(await _svc.GetByProjectIdAsync(projectId));

    [AllowAnonymous]
    [HttpGet("by-skill/{skillId}")]
    public async Task<IActionResult> BySkill(string skillId) => Ok(await _svc.GetBySkillIdAsync(skillId));

    [Authorize(Roles = "Admin,User")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProjectSkill dto) => Ok(await _svc.CreateAsync(dto));

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id) => Ok(await _svc.DeleteAsync(id));
}
