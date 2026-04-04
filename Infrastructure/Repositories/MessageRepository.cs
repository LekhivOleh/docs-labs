using docs_project.Application.Interfaces.Repositories;
using docs_project.Domain.Entities;
using docs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace docs_project.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;

        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Message message)
        {
            _context.Messages.Update(message);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var m = await _context.Messages.FindAsync(id);
            if (m is null) return;
            _context.Messages.Remove(m);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Message>> GetByChatIdAsync(Guid chatId)
        {
            return await _context.Messages
                .Where(m => m.ChatId == chatId)
                .ToListAsync();
        }
    }
}
