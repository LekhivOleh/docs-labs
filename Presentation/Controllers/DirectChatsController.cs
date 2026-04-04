using docs_project.Application.Dto.Upsert;
using docs_project.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace docs_project.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectChatsController : ControllerBase
    {
        private readonly IChatService _chatService;

        public DirectChatsController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(DirectChatUpsertDto dto)
        {
            var created = await _chatService.CreateDirectAsync(dto);
            return CreatedAtAction("GetById", "Chats", new { id = created.Id }, created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDirects()
        {
            var list = await _chatService.GetAllDirectsAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDirectById(Guid id)
        {
            var d = await _chatService.GetDirectByIdAsync(id);
            if (d is null) return NotFound();
            return Ok(d);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDirect(Guid id)
        {
            await _chatService.DeleteAsync(id);
            return NoContent();
        }
    }
}
