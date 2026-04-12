namespace docs_project.Domain.Entities
{
    public abstract class Chat
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
