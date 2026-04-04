using docs_project.Domain.Entities;

namespace docs_project.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task<List<User>> GetAllAsync();
        Task<(List<User> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
        Task<User?> GetByCredentialsAsync(string usernameOrEmail, string password);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
        Task DeleteAllAsync();
    }
}
