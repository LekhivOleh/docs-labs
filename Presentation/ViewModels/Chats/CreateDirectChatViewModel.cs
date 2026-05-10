using System.ComponentModel.DataAnnotations;

namespace docs_project.Presentation.ViewModels.Chats;

public class CreateDirectChatViewModel
{
    [Required]
    public Guid FirstUserId { get; set; }

    [Required]
    public Guid SecondUserId { get; set; }
}

