using docs_project.Application.Dto.Auth;
using docs_project.Application.Interfaces.Services;
using docs_project.Application.Dto.Upsert;
using Microsoft.AspNetCore.Mvc;

namespace docs_project.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var res = await _authService.AuthenticateAsync(req);
            if (res == null) return Unauthorized();
            return Ok(res);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserUpsertDto dto)
        {
            var u = new Domain.Entities.User
            {
                Id = dto.Id ?? Guid.NewGuid(),
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Id == null ? dto.Username + "123" : "" //only demo
            };
            await _userService.CreateAsync(u);
            return CreatedAtAction(nameof(Register), new { id = u.Id }, new { u.Id });
        }
    }
}
