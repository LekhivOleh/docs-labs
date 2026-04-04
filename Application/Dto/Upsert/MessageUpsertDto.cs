using System.ComponentModel.DataAnnotations;

namespace docs_project.Application.Dto.Upsert
{
    public class MessageUpsertDto
    {
        public Guid? Id { get; set; }
        [Required]
        public string? Content { get; set; }
        public Guid UserId { get; set; }
        public Guid ChatId { get; set; }
        // IsRead removed
    }
}
