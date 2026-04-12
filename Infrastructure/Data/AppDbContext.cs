using docs_project.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace docs_project.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<GroupChat> GroupChats { get; set; }
        public DbSet<DirectChat> DirectChats { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>()
                .HasDiscriminator<string>("ChatType")
                .HasValue<GroupChat>("Group")
                .HasValue<DirectChat>("Direct");

            modelBuilder.Entity<Message>()
                .HasOne(m => m.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.UserId);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId);

            modelBuilder.Entity<Chat>()
                .HasMany(c => c.Users)
                .WithMany(u => u.Chats)
                .UsingEntity(j => j.ToTable("UserChats"));
        }
    }
}
