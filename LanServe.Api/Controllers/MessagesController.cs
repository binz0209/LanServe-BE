using LanServe.Application.Interfaces.Services;
using LanServe.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanServe.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _svc;
    public MessagesController(IMessageService svc) { _svc = svc; }

    [Authorize]
    [HttpGet("thread/{conversationKey}")]
    public async Task<IActionResult> Thread(string conversationKey)
        => Ok(await _svc.GetByConversationAsync(conversationKey));

    [Authorize]
    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> ByProject(string projectId)
        => Ok(await _svc.GetByProjectAsync(projectId));

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Send([FromBody] Message dto) => Ok(await _svc.SendAsync(dto));

    [Authorize]
    [HttpPost("{id}/read")]
    public async Task<IActionResult> MarkRead(string id) => Ok(await _svc.MarkAsReadAsync(id));
}
