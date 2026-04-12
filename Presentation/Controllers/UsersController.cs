using docs_project.Application.Dto.Read;
using docs_project.Application.Dto.Summary;
using docs_project.Application.Interfaces.Services;
using docs_project.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace docs_project.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService userService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            var created = await userService.CreateAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await userService.GetByIdAsync(id);
            if (user is null) return NotFound();

            var dto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };

            if (user.Chats != null)
            {
                foreach (var c in user.Chats)
                {
                    var summary = new ChatSummaryDto
                    {
                        Id = c.Id,
                        Type = c.GetType().Name,
                        Title = c is GroupChat gc ? gc.Title : null,
                        UserCount = c.Users?.Count ?? 0,
                        MessageCount = c.Messages?.Count ?? 0
                    };
                    dto.Chats.Add(summary);
                }
            }

            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0 || pageSize > 1000) pageSize = 50;

            var (items, total) = await userService.GetPagedAsync(page, pageSize);
            return Ok(new { items, total, page, pageSize });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await userService.DeleteAsync(id);
            return NoContent();
        }

        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAll()
        {
            await userService.DeleteAllAsync();
            return NoContent();
        }

        [HttpPost("import")]
        [RequestSizeLimit(104857600)] // 100 mb
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file is null || file.Length == 0)
                return BadRequest("File is required");
            await using var stream = file.OpenReadStream();
            var imported = await userService.ImportFromCsvAsync(stream);
            return Ok(new { imported });
        }
    }
}
