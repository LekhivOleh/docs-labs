using docs_project.Application.Dto.Summary;
using System.ComponentModel.DataAnnotations;

namespace docs_project.Application.Dto.Read
{
    public class ChatDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        public string? Type { get; set; }
        public string? Title { get; set; }
        public Guid? OwnerId { get; set; }
        public Guid? FirstUserId { get; set; }
        public Guid? SecondUserId { get; set; }
        public List<UserSummaryDto> Users { get; set; } = new();
        public List<MessageDto> Messages { get; set; } = new();
    }
}
