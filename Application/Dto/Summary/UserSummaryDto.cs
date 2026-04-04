using System.ComponentModel.DataAnnotations;

namespace docs_project.Application.Dto.Summary
{
    public class UserSummaryDto
    {
        public Guid Id { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Email { get; set; }
    }
}
