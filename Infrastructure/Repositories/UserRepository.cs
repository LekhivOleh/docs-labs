using docs_project.Application.Interfaces.Repositories;
using docs_project.Domain.Entities;
using docs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace docs_project.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .Include(u => u.Messages)
                .Include(u => u.Chats)
                    .ThenInclude(c => c.Users)
                .Include(u => u.Chats)
                    .ThenInclude(c => c.Messages)
                .FirstOrDefaultAsync();
        }

        public async Task<User?> GetByCredentialsAsync(string usernameOrEmail, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => (u.Username == usernameOrEmail || u.Email == usernameOrEmail) && u.Password == password);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null) return;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllAsync()
        {
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM Users");
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<(List<User> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
        {
            var total = await _context.Users.CountAsync();
            var items = await _context.Users
                .OrderBy(u => u.Username)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (items, total);
        }
    }
}
