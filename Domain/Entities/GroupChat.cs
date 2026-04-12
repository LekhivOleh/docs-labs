namespace docs_project.Domain.Entities
{
    public class GroupChat : Chat
    {
        public string Title { get; set; }
        public Guid OwnerId { get; set; }
    }
}
