using System.ComponentModel.DataAnnotations;

namespace docs_project.Presentation.ViewModels.Chats;

public class SendMessageViewModel
{
    [Required]
    public Guid ChatId { get; set; }

    [Required]
    [StringLength(1000)]
    public string Content { get; set; } = string.Empty;
}

