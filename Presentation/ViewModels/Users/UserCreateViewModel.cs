using System.ComponentModel.DataAnnotations;

namespace docs_project.Presentation.ViewModels.Users;

public class UserCreateViewModel
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
}

