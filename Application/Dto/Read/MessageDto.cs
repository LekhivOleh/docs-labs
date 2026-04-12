using System.ComponentModel.DataAnnotations;

namespace docs_project.Application.Dto.Read
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        public string? Content { get; set; }
        public Guid UserId { get; set; }
        public Guid ChatId { get; set; }
        // IsRead property removed
    }
}
