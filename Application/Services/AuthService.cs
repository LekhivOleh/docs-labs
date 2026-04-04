using docs_project.Application.Dto.Auth;
using docs_project.Application.Interfaces.Repositories;
using docs_project.Application.Interfaces.Services;

namespace docs_project.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<LoginResponse?> AuthenticateAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByCredentialsAsync(request.UsernameOrEmail, request.Password);
            if (user == null) return null;

            var token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{user.Id}:{DateTime.UtcNow.Ticks}"));
            return new LoginResponse { Token = token, UserId = user.Id };
        }
    }
}
