using LanServe.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanServe.Infrastructure.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<Message> CreateAsync(Message message)
        {
            await _messageRepository.AddAsync(message);
            return message;
        }

        public async Task<Message?> GetByIdAsync(string id) => await _messageRepository.GetByIdAsync(id);

        public async Task<IEnumerable<Message>> GetByConversationIdAsync(string conversationId)
            => await _messageRepository.GetByConversationIdAsync(conversationId);

        public async Task DeleteAsync(string id) => await _messageRepository.DeleteAsync(id);
    }
}
