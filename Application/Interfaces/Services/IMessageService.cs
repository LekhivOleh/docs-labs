using docs_project.Application.Dto.Read;
using docs_project.Application.Dto.Upsert;

namespace docs_project.Application.Interfaces.Services
{
    public interface IMessageService
    {
        Task<MessageDto> CreateAsync(MessageUpsertDto dto);
        Task<List<MessageDto>> GetByChatIdAsync(Guid chatId);
        Task UpdateAsync(MessageDto dto);
        Task DeleteAsync(Guid id);
    }
}
