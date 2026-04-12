using System.ComponentModel.DataAnnotations;

namespace docs_project.Application.Dto.Summary
{
    public class ChatSummaryDto
    {
        public Guid Id { get; set; }
        [Required]
        public string? Type { get; set; }
        public string? Title { get; set; }
        public int UserCount { get; set; }
        public int MessageCount { get; set; }
    }
}
