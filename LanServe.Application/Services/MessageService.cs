// LanServe.Application/Services/MessageService.cs
using LanServe.Application.Interfaces.Repositories;
using LanServe.Application.Interfaces.Services;
using LanServe.Domain.Entities;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _repo;
    public MessageService(IMessageRepository repo) => _repo = repo;

    public Task<IEnumerable<Message>> GetByConversationAsync(string conversationKey)
        => _repo.GetByConversationAsync(conversationKey).ContinueWith(t => (IEnumerable<Message>)t.Result);

    public Task<IEnumerable<Message>> GetByProjectAsync(string projectId)
        => _repo.GetByProjectAsync(projectId).ContinueWith(t => (IEnumerable<Message>)t.Result);

    public Task<IEnumerable<Message>> GetByUserAsync(string userId)
        => _repo.GetByUserAsync(userId).ContinueWith(t => (IEnumerable<Message>)t.Result);

    public async Task<IEnumerable<(string ConversationKey, string PartnerId, string LastMessage, DateTime LastAt, int UnreadCount)>>
        GetConversationsForUserAsync(string userId)
        => await _repo.GetConversationsForUserAsync(userId);

    public async Task<Message> SendAsync(Message dto)
    {
        if (string.IsNullOrWhiteSpace(dto.ConversationKey)
            && !string.IsNullOrWhiteSpace(dto.SenderId)
            && !string.IsNullOrWhiteSpace(dto.ReceiverId))
        {
            dto.ConversationKey = BuildConversationKey(dto.SenderId, dto.ReceiverId);
        }
        dto.CreatedAt = dto.CreatedAt == default ? DateTime.UtcNow : dto.CreatedAt;
        dto.IsRead = false;

        return await _repo.AddAsync(dto);
    }

    public Task<bool> MarkAsReadAsync(string id) => _repo.MarkAsReadAsync(id);

    private static string BuildConversationKey(string a, string b)
    {
        var arr = new[] { a, b };
        Array.Sort(arr, StringComparer.Ordinal);
        return $"{arr[0]}_{arr[1]}";
    }
}
