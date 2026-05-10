using docs_project.Domain.Entities;

namespace docs_project.Application.Interfaces.Repositories
{
    public interface IChatRepository
    {
        Task AddAsync(Chat chat);
        Task<Chat?> GetByIdAsync(Guid id);
        Task<List<Chat>> GetAllAsync();
        Task UpdateAsync(Chat chat);
        Task AddUserToGroupAsync(Guid groupId, Guid userId);
        Task DeleteAsync(Guid id);
        Task<List<GroupChat>> GetAllGroupsAsync();
        Task<GroupChat?> GetGroupByIdAsync(Guid id);
        Task<List<DirectChat>> GetAllDirectsAsync();
        Task<DirectChat?> GetDirectByIdAsync(Guid id);
    }
}
