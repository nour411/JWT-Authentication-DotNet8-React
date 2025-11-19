using AuthenticationCL.Domain.Entities;
using AuthenticationCL.DTOs;

namespace AuthenticationCL.IServices
{
    public interface IAuthService
    {
        Task<string> GenerateJwt(User user);
        Task<string> GenerateRefreshTokenAsync(string email);
        Task<AuthResponse> RegisterAsync(RegisterRequestDTO request);
        Task<AuthResponse> LoginAsync(LogInRequestDTO request);
    }
}
