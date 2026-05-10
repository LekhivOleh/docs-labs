using System.ComponentModel.DataAnnotations;

namespace docs_project.Presentation.ViewModels.Chats;

public class CreateGroupChatViewModel
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public Guid OwnerId { get; set; }

    public List<Guid> UserIds { get; set; } = new();
}

