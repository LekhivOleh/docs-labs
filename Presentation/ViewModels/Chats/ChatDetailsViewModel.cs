using docs_project.Application.Dto.Read;

namespace docs_project.Presentation.ViewModels.Chats;

public class ChatDetailsViewModel
{
    public ChatDto Chat { get; set; } = new();
    public SendMessageViewModel NewMessage { get; set; } = new();
    public string? Error { get; set; }
}

