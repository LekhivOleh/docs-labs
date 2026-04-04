using docs_project.Domain.Entities;
using docs_project.Application.Dto.Upsert;
using docs_project.Application.Dto.Read;

namespace docs_project.Application.Interfaces.Services
{
    public interface IChatService
    {
        Task<ChatDto> CreateGroupAsync(GroupChatUpsertDto dto);
        Task<ChatDto> CreateDirectAsync(DirectChatUpsertDto dto);
        Task<ChatDto?> GetByIdAsync(Guid id);
        Task<List<ChatDto>> GetAllAsync();
        Task AddUserToGroupAsync(Guid groupId, Guid userId);
        Task DeleteAsync(Guid id);
        Task<List<ChatDto>> GetAllGroupsAsync();
        Task<ChatDto?> GetGroupByIdAsync(Guid id);
        Task<List<ChatDto>> GetAllDirectsAsync();
        Task<ChatDto?> GetDirectByIdAsync(Guid id);
    }
}
