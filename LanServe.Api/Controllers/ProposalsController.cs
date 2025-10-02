using LanServe.Application.DTOs;
using LanServe.Application.Interfaces.Services;
using LanServe.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanServe.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProposalsController : ControllerBase
    {
        private readonly IProposalService _svc;

        public ProposalsController(IProposalService svc)
        {
            _svc = svc;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
            => Ok(await _svc.GetByIdAsync(id));

        [HttpGet("by-project/{projectId}")]
        public async Task<IActionResult> ByProject(string projectId)
            => Ok(await _svc.GetByProjectIdAsync(projectId));

        [HttpGet("by-freelancer/{freelancerId}")]
        public async Task<IActionResult> ByFreelancer(string freelancerId)
            => Ok(await _svc.GetByFreelancerIdAsync(freelancerId));

        // ================== CREATE PROPOSAL ==================
        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProposalDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ProjectId) || string.IsNullOrWhiteSpace(dto.FreelancerId))
                return BadRequest("ProjectId and FreelancerId are required");

            var proposal = new Proposal
            {
                ProjectId = dto.ProjectId,
                FreelancerId = dto.FreelancerId,
                CoverLetter = dto.CoverLetter ?? "",
                BidAmount = dto.BidAmount,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            var created = await _svc.CreateAsync(proposal);
            return Ok(created);
        }

        // ================== UPDATE STATUS ==================
        [Authorize(Roles = "User,Admin")]
        [HttpPut("{id}/status/{status}")]
        public async Task<IActionResult> UpdateStatus(string id, string status)
            => Ok(await _svc.UpdateStatusAsync(id, status));

        // ================== DELETE ==================
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
            => Ok(await _svc.DeleteAsync(id));
    }
}
