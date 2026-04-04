using System.ComponentModel.DataAnnotations;

namespace docs_project.Application.Dto.Upsert
{
    public class UserUpsertDto
    {
        public Guid? Id { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Email { get; set; }
    }
}
