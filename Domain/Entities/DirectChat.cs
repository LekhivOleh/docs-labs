namespace docs_project.Domain.Entities
{
    public class DirectChat : Chat
    {
        public Guid FirstUserId { get; set; }
        public Guid SecondUserId { get; set; }
    }
}
