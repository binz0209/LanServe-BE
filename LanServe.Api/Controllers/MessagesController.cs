// LanServe.Api/Controllers/MessagesController.cs
using LanServe.Application.Interfaces.Services;
using LanServe.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Security.Claims;

namespace LanServe.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _svc;
    public MessagesController(IMessageService svc) { _svc = svc; }

    private string? GetUserId()
        => User.FindFirst(ClaimTypes.NameIdentifier)?.Value
           ?? User.FindFirst("sub")?.Value
           ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

    // NEW: trả toàn bộ tin nhắn của user
    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> MyMessages()
    {
        var userId = GetUserId();
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

        var list = await _svc.GetByUserAsync(userId);
        return Ok(list);
    }

    // NEW: trả danh sách hội thoại đã group (không tạo DTO ngoài)
    [Authorize]
    [HttpGet("my-conversations")]
    public async Task<IActionResult> MyConversations()
    {
        var userId = GetUserId();
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

        var convos = await _svc.GetConversationsForUserAsync(userId);

        // Trả object ẩn danh đúng shape cho FE
        var result = convos.Select(c => new {
            conversationKey = c.ConversationKey,
            partnerId = c.PartnerId,
            lastMessage = c.LastMessage,
            lastAt = c.LastAt,
            unreadCount = c.UnreadCount
        });

        return Ok(result);
    }

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
    public async Task<IActionResult> Send([FromBody] Dictionary<string, object> body)
    {
        var senderId = GetUserId();
        if (string.IsNullOrEmpty(senderId)) return Unauthorized();

        // Lấy receiverId & text từ payload
        var receiverId = body.ContainsKey("receiverId") ? body["receiverId"]?.ToString() : null;
        var text = body.ContainsKey("text") ? body["text"]?.ToString() : null;

        if (string.IsNullOrWhiteSpace(receiverId) || string.IsNullOrWhiteSpace(text))
            return BadRequest("receiverId và text là bắt buộc.");

        var msg = new Message
        {
            Id = ObjectId.GenerateNewId().ToString(), // Mongo sẽ dùng
            SenderId = senderId,
            ReceiverId = receiverId!,
            Text = text!,
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
            ConversationKey = string.Compare(senderId, receiverId, StringComparison.Ordinal) < 0
                ? $"{senderId}:{receiverId}"
                : $"{receiverId}:{senderId}"
        };

        var saved = await _svc.SendAsync(msg);
        return Ok(saved);
    }


    [Authorize]
    [HttpPost("{id}/read")]
    public async Task<IActionResult> MarkRead(string id)
        => Ok(await _svc.MarkAsReadAsync(id));
}
