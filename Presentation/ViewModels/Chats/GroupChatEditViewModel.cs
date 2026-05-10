using System.ComponentModel.DataAnnotations;

namespace docs_project.Presentation.ViewModels.Chats;

public class GroupChatEditViewModel
{
    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;
}
