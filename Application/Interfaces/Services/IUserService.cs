using docs_project.Domain.Entities;

namespace docs_project.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<User> CreateAsync(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task<List<User>> GetAllAsync();
        Task<(List<User> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
        Task<int> ImportFromCsvAsync(System.IO.Stream stream);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
        Task DeleteAllAsync();
    }
}
