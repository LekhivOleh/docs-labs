using docs_project.Application.Dto.Summary;
using System.ComponentModel.DataAnnotations;

namespace docs_project.Application.Dto.Read
{
    public class UserDto
    {
        public Guid Id { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Email { get; set; }
        public List<ChatSummaryDto> Chats { get; set; } = new();
        public List<Guid> Messages { get; set; } = new();
    }
}
