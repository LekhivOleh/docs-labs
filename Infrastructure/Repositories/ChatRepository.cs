using docs_project.Application.Interfaces.Repositories;
using docs_project.Domain.Entities;
using docs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace docs_project.Infrastructure.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly AppDbContext _context;

        public ChatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Chat chat)
        {
            await _context.Chats.AddAsync(chat);
            await _context.SaveChangesAsync();
        }

        public async Task<Chat?> GetByIdAsync(Guid id)
        {
            return await _context.Chats
                .Include(c => c.Users)
                .Include(c => c.Messages)
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Chat>> GetAllAsync()
        {
            return await _context.Chats
                .Include(c => c.Users)
                .Include(c => c.Messages)
                    .ThenInclude(m => m.User)
                .ToListAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var chat = await _context.Chats.FindAsync(id);
            if (chat is null) return;
            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();
        }

        public async Task<List<GroupChat>> GetAllGroupsAsync()
        {
            return await _context.GroupChats
                .Include(g => g.Users)
                .Include(g => g.Messages)
                    .ThenInclude(m => m.User)
                .ToListAsync();
        }

        public async Task<GroupChat?> GetGroupByIdAsync(Guid id)
        {
            return await _context.GroupChats
                .Include(g => g.Users)
                .Include(g => g.Messages)
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<List<DirectChat>> GetAllDirectsAsync()
        {
            return await _context.DirectChats
                .Include(d => d.Users)
                .Include(d => d.Messages)
                    .ThenInclude(m => m.User)
                .ToListAsync();
        }

        public async Task<DirectChat?> GetDirectByIdAsync(Guid id)
        {
            return await _context.DirectChats
                .Include(d => d.Users)
                .Include(d => d.Messages)
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task UpdateAsync(Chat chat)
        {
            _context.Chats.Update(chat);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserToGroupAsync(Guid groupId, Guid userId)
        {
            var group = await _context.GroupChats
                .Include(g => g.Users)
                .FirstOrDefaultAsync(g => g.Id == groupId);
            if (group is null) return;

            var user = await _context.Users.FindAsync(userId);
            if (user is null) return;

            if (!group.Users.Any(u => u.Id == userId))
            {
                group.Users.Add(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
