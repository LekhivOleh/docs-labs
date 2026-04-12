using docs_project.Application.Dto.Upsert;
using docs_project.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace docs_project.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MessageUpsertDto dto)
        {
            var created = await _messageService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByChat), new { chatId = created.ChatId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] docs_project.Application.Dto.Read.MessageDto dto)
        {
            if (id != dto.Id) return BadRequest();
            await _messageService.UpdateAsync(dto);
            return NoContent();
        }

        [HttpGet("chat/{chatId}")]
        public async Task<IActionResult> GetByChat(Guid chatId)
        {
            var messages = await _messageService.GetByChatIdAsync(chatId);
            return Ok(messages);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _messageService.DeleteAsync(id);
            return NoContent();
        }
    }
}
