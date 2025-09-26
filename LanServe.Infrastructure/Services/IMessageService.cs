using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanServe.Infrastructure.Services
{
    public interface IMessageService
    {
        Task<Message> CreateAsync(Message message);
        Task<Message?> GetByIdAsync(string id);
        Task<IEnumerable<Message>> GetByConversationIdAsync(string conversationId);
        Task DeleteAsync(string id);
    }
}
