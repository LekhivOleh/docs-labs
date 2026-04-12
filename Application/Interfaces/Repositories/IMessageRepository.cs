using docs_project.Domain.Entities;

namespace docs_project.Application.Interfaces.Repositories
{
    public interface IMessageRepository
    {
        Task AddAsync(Message message);
        Task<List<Message>> GetByChatIdAsync(Guid chatId);
        Task UpdateAsync(Message message);
        Task DeleteAsync(Guid id);
    }
}
