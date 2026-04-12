namespace docs_project.Domain.Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string Content { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid ChatId { get; set; }
        public Chat Chat { get; set; }

    }
}
