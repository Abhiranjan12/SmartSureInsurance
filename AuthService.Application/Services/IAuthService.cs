using AuthService.Application.DTOs;

namespace AuthService.Application.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterRequestDto request);
        Task<string> LoginAsync(LoginRequestDto request);
        Task<AuthResponseDto> VerifyLoginOtpAsync(VerifyOtpDto request);
        Task ResendOtpAsync(ResendOtpDto request);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
        Task RevokeTokenAsync(Guid userId);
    }
}
