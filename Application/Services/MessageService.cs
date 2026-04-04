using docs_project.Application.Interfaces.Repositories;
using docs_project.Application.Interfaces.Services;
using docs_project.Domain.Entities;
using docs_project.Application.Dto.Upsert;
using docs_project.Application.Dto.Read;


namespace docs_project.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IChatRepository _chatRepository;

        public MessageService(IMessageRepository messageRepository, IChatRepository chatRepository)
        {
            _messageRepository = messageRepository;
            _chatRepository = chatRepository;
        }

        public async Task UpdateAsync(MessageDto dto)
        {
            var m = new Message
            {
                Id = dto.Id,
                CreatedAt = dto.CreatedAt,
                ModifiedAt = DateTime.UtcNow,
                Content = dto.Content,
                UserId = dto.UserId,
                ChatId = dto.ChatId,
            };
            await _messageRepository.UpdateAsync(m);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _messageRepository.DeleteAsync(id);
        }

        public async Task<MessageDto> CreateAsync(MessageUpsertDto dto)
        {
            var chat = await _chatRepository.GetByIdAsync(dto.ChatId);
            if (chat is null)
                throw new InvalidOperationException("Chat not found");

            var userInChat = chat.Users?.Any(u => u.Id == dto.UserId) ?? false;
            if (!userInChat)
                throw new InvalidOperationException("User is not a member of the chat");

            var message = new Message
            {
                Id = dto.Id ?? Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                Content = dto.Content,
                UserId = dto.UserId,
                ChatId = dto.ChatId
            };

            await _messageRepository.AddAsync(message);

            return new MessageDto
            {
                Id = message.Id,
                CreatedAt = message.CreatedAt,
                Content = message.Content,
                UserId = message.UserId,
                ChatId = message.ChatId,
            };
        }

        public async Task<List<MessageDto>> GetByChatIdAsync(Guid chatId)
        {
            var messages = await _messageRepository.GetByChatIdAsync(chatId);
            return messages.Select(m => new MessageDto
            {
                Id = m.Id,
                CreatedAt = m.CreatedAt,
                Content = m.Content,
                UserId = m.UserId,
                ChatId = m.ChatId,
            }).ToList();
        }
    }
}
