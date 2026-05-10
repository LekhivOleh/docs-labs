using System.Security.Claims;
using docs_project.Application.Dto.Summary;
using docs_project.Application.Dto.Upsert;
using docs_project.Application.Interfaces.Services;
using docs_project.Presentation.ViewModels.Chats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace docs_project.Presentation.Controllers;

[Authorize]
[Route("Chats")]
public class ChatsMvcController : Controller
{
    private readonly IChatService _chatService;
    private readonly IMessageService _messageService;
    private readonly IUserService _userService;

    public ChatsMvcController(IChatService chatService, IMessageService messageService, IUserService userService)
    {
        _chatService = chatService;
        _messageService = messageService;
        _userService = userService;
    }

    private async Task<List<UserSummaryDto>> GetUserSummariesAsync()
    {
        var users = await _userService.GetAllAsync();
        return users.Select(u => new UserSummaryDto { Id = u.Id, Username = u.Username, Email = u.Email }).ToList();
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var allChats = await _chatService.GetAllAsync();
        var users = await GetUserSummariesAsync();

        Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var currentUserId);
        var yourChats = allChats.Where(c => c.Users.Any(u => u.Id == currentUserId)).ToList();

        return View(new ChatsIndexViewModel
        {
            Chats = allChats,
            YourChats = yourChats,
            Users = users
        });
    }

    [HttpPost("direct")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateDirect(CreateDirectChatViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", new ChatsIndexViewModel
            {
                Chats = await _chatService.GetAllAsync(),
                Users = await GetUserSummariesAsync(),
                NewDirect = model
            });
        }

        await _chatService.CreateDirectAsync(new DirectChatUpsertDto
        {
            FirstUserId = model.FirstUserId,
            SecondUserId = model.SecondUserId
        });
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("group")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateGroup(CreateGroupChatViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", new ChatsIndexViewModel
            {
                Chats = await _chatService.GetAllAsync(),
                Users = await GetUserSummariesAsync(),
                NewGroup = model
            });
        }

        await _chatService.CreateGroupAsync(new GroupChatUpsertDto
        {
            Title = model.Title,
            OwnerId = model.OwnerId,
            UserIds = model.UserIds
        });
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Details(Guid id)
    {
        var chat = await _chatService.GetByIdAsync(id);
        if (chat is null)
        {
            return NotFound();
        }

        return View(new ChatDetailsViewModel
        {
            Chat = chat,
            NewMessage = new SendMessageViewModel { ChatId = id }
        });
    }

    [HttpPost("message")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendMessage([Bind(Prefix = "NewMessage")] SendMessageViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var chat = await _chatService.GetByIdAsync(model.ChatId);
            if (chat is null)
            {
                return NotFound();
            }

            return View("Details", new ChatDetailsViewModel
            {
                Chat = chat,
                NewMessage = model,
                Error = "Message content is required."
            });
        }

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return Forbid();
        }

        try
        {
            await _messageService.CreateAsync(new MessageUpsertDto
            {
                ChatId = model.ChatId,
                UserId = userId,
                Content = model.Content
            });
        }
        catch (InvalidOperationException ex)
        {
            var chat = await _chatService.GetByIdAsync(model.ChatId);
            if (chat is null)
            {
                return NotFound();
            }

            return View("Details", new ChatDetailsViewModel
            {
                Chat = chat,
                NewMessage = model,
                Error = ex.Message
            });
        }

        return RedirectToAction(nameof(Details), new { id = model.ChatId });
    }

    [HttpGet("{id:guid}/edit")]
    public async Task<IActionResult> EditGroup(Guid id)
    {
        var chat = await _chatService.GetGroupByIdAsync(id);
        if (chat is null) return NotFound();
        return View(new GroupChatEditViewModel { Id = id, Title = chat.Title ?? string.Empty });
    }

    [HttpPost("{id:guid}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditGroup(Guid id, GroupChatEditViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        await _chatService.UpdateGroupTitleAsync(id, model.Title);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:guid}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _chatService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}

