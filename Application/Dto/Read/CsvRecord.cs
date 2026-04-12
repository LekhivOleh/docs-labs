using System.ComponentModel.DataAnnotations;

namespace docs_project.Application.Dto.Read
{
    public class CsvRecord
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
