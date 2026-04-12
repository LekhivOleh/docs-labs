using docs_project.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace docs_project.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatsController(IChatService chatService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var chat = await chatService.GetByIdAsync(id);
            if (chat is null)
                return NotFound();
            return Ok(chat);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var chats = await chatService.GetAllAsync();
            return Ok(chats);
        }
    }
}
