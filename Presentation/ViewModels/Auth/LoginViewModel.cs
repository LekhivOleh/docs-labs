using System.ComponentModel.DataAnnotations;

namespace docs_project.Presentation.ViewModels.Auth;

public class LoginViewModel
{
    [Required]
    public string UsernameOrEmail { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}

