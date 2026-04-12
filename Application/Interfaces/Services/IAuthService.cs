using docs_project.Application.Dto.Auth;

namespace docs_project.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponse?> AuthenticateAsync(LoginRequest request);
    }
}
