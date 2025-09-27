using LanServe.Application.Interfaces.Services;
using LanServe.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanServe.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProposalsController : ControllerBase
{
    private readonly IProposalService _svc;
    public ProposalsController(IProposalService svc) { _svc = svc; }

    [Authorize]
    [HttpGet("{id}")] public async Task<IActionResult> GetById(string id) => Ok(await _svc.GetByIdAsync(id));

    [Authorize]
    [HttpGet("by-project/{projectId}")]
    public async Task<IActionResult> ByProject(string projectId) => Ok(await _svc.GetByProjectIdAsync(projectId));

    [Authorize]
    [HttpGet("by-freelancer/{freelancerId}")]
    public async Task<IActionResult> ByFreelancer(string freelancerId) => Ok(await _svc.GetByFreelancerIdAsync(freelancerId));

    [Authorize(Roles = "User,Admin")]
    [HttpPost] public async Task<IActionResult> Create([FromBody] Proposal dto) => Ok(await _svc.CreateAsync(dto));

    public record UpdateStatusDto(string Status);
    [Authorize]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(string id, [FromBody] UpdateStatusDto dto)
        => Ok(await _svc.UpdateStatusAsync(id, dto.Status));

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")] public async Task<IActionResult> Delete(string id) => Ok(await _svc.DeleteAsync(id));
}
