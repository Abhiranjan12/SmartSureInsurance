using AuthService.Application.DTOs;
using AuthService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var message = await _authService.RegisterAsync(request);
            return Ok(new { message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);

            if (result.StartsWith("ADMIN_DIRECT:"))
            {
                var parts = result.Replace("ADMIN_DIRECT:", "").Split("|");
                return Ok(new AuthResponseDto
                {
                    AccessToken = parts[0],
                    RefreshToken = parts[1],
                    Role = parts[2],
                    UserId = Guid.Parse(parts[3]),
                    FullName = parts[4]
                });
            }

            return Ok(new { message = result });
        }

        [HttpPost("verify-login-otp")]
        public async Task<IActionResult> VerifyLoginOtp([FromBody] VerifyOtpDto request)
        {
            var result = await _authService.VerifyLoginOtpAsync(request);
            return Ok(result);
        }

        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp([FromBody] ResendOtpDto request)
        {
            await _authService.ResendOtpAsync(request);
            return Ok(new { message = "OTP resent successfully." });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto request)
        {
            var result = await _authService.RefreshTokenAsync(request.RefreshToken);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke()
        {
            string? userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr == null)
                userIdStr = User.FindFirstValue("sub");

            if (userIdStr == null)
                throw new UnauthorizedAccessException("User identity not found.");

            Guid userId = Guid.Parse(userIdStr);
            await _authService.RevokeTokenAsync(userId);
            return NoContent();
        }
    }
}
