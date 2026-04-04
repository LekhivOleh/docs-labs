using docs_project.Application.Dto.Upsert;
using docs_project.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace docs_project.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupChatsController : ControllerBase
    {
        private readonly IChatService _chatService;

        public GroupChatsController(IChatService chatService)
        {
            _chatService = chatService;
        }
        [HttpPost]
        public async Task<IActionResult> Create(GroupChatUpsertDto dto)
        {
            var created = await _chatService.CreateGroupAsync(dto);
            return CreatedAtAction("GetById", "Chats", new { id = created.Id }, created);
        }

        [HttpPost("{groupId}/users/{userId}")]
        public async Task<IActionResult> AddUser(Guid groupId, Guid userId)
        {
            await _chatService.AddUserToGroupAsync(groupId, userId);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGroups()
        {
            var list = await _chatService.GetAllGroupsAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupById(Guid id)
        {
            var group = await _chatService.GetGroupByIdAsync(id);
            if (group is null) return NotFound();
            return Ok(group);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(Guid id)
        {
            await _chatService.DeleteAsync(id);
            return NoContent();
        }
    }
}
