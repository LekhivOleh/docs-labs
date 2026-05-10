using docs_project.Application.Dto.Read;
using docs_project.Application.Dto.Summary;

namespace docs_project.Presentation.ViewModels.Chats;

public class ChatsIndexViewModel
{
    public List<ChatDto> Chats { get; set; } = new();
    public List<ChatDto> YourChats { get; set; } = new();
    public List<UserSummaryDto> Users { get; set; } = new();
    public CreateDirectChatViewModel NewDirect { get; set; } = new();
    public CreateGroupChatViewModel NewGroup { get; set; } = new();
}

