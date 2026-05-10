using System.ComponentModel.DataAnnotations;

namespace docs_project.Presentation.ViewModels.Users;

public class UserEditViewModel
{
    public Guid Id { get; set; }

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
