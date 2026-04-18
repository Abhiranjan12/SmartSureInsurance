using AuthService.Application.DTOs;
using AuthService.Application.Services;
using AuthService.Domain.Entities;
using AuthService.Domain.Repositories;
using MassTransit;
using SmartSure.Contracts.Events;

namespace AuthService.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly IPublishEndpoint _publishEndpoint;

        public AuthService(IUserRepository userRepo, IJwtService jwtService, IEmailService emailService, IPublishEndpoint publishEndpoint)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
            _emailService = emailService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<string> RegisterAsync(RegisterRequestDto request)
        {
            if (await _userRepo.EmailExistsAsync(request.Email))
                throw new InvalidOperationException("Email already registered.");

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Age = request.Age,
                Role = "Customer",
                IsEmailVerified = true
            };

            await _userRepo.AddAsync(user);

            try
            {
                await _publishEndpoint.Publish(new UserRegisteredEvent
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = user.Role
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARNING] Failed to publish UserRegisteredEvent for {user.Email}: {ex.Message}");
            }

            return "Registration successful. You can now login.";
        }

        public async Task<string> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepo.GetByEmailAsync(request.Email);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password.");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");

            // Admin ke liye OTP skip — seedha JWT
            if (user.Role == "Admin")
            {
                var refreshToken = _jwtService.GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
                await _userRepo.UpdateAsync(user);

                return "ADMIN_DIRECT:" + _jwtService.GenerateAccessToken(user) + "|" + refreshToken + "|" + user.Role + "|" + user.Id + "|" + user.FullName;
            }

            var otp = GenerateOtp();
            user.OtpCode = otp;
            user.OtpExpiry = DateTime.UtcNow.AddMinutes(10);
            await _userRepo.UpdateAsync(user);

            Console.WriteLine($"\n===========================================");
            Console.WriteLine($"[DEV-LOCAL] OTP FOR {user.Email}: {otp}");
            Console.WriteLine($"===========================================\n");

            try
            {
                await _emailService.SendOtpAsync(user.Email, user.FullName, otp);
            }
            catch (Exception)
            {
                // Email failed — OTP saved in DB
            }

            return "OTP sent to your email. Please verify to complete login.";
        }

        public async Task<AuthResponseDto> VerifyLoginOtpAsync(VerifyOtpDto request)
        {
            var user = await _userRepo.GetByEmailAsync(request.Email);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            if (user.OtpCode != request.OtpCode)
                throw new UnauthorizedAccessException("Invalid OTP.");

            if (user.OtpExpiry < DateTime.UtcNow)
                throw new UnauthorizedAccessException("OTP has expired. Please login again.");

            user.OtpCode = null;
            user.OtpExpiry = null;

            var refreshToken = _jwtService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _userRepo.UpdateAsync(user);

            return new AuthResponseDto
            {
                AccessToken = _jwtService.GenerateAccessToken(user),
                RefreshToken = refreshToken,
                Role = user.Role,
                UserId = user.Id,
                FullName = user.FullName
            };
        }

        public async Task ResendOtpAsync(ResendOtpDto request)
        {
            var user = await _userRepo.GetByEmailAsync(request.Email);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var otp = GenerateOtp();
            user.OtpCode = otp;
            user.OtpExpiry = DateTime.UtcNow.AddMinutes(10);

            await _userRepo.UpdateAsync(user);
            await _emailService.SendOtpAsync(user.Email, user.FullName, otp);
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userRepo.GetByRefreshTokenAsync(refreshToken);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");

            if (user.RefreshTokenExpiry < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token expired.");

            var newRefreshToken = _jwtService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _userRepo.UpdateAsync(user);

            return new AuthResponseDto
            {
                AccessToken = _jwtService.GenerateAccessToken(user),
                RefreshToken = newRefreshToken,
                Role = user.Role,
                UserId = user.Id,
                FullName = user.FullName
            };
        }

        public async Task RevokeTokenAsync(Guid userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await _userRepo.UpdateAsync(user);
        }

        private static string GenerateOtp()
        {
            return Random.Shared.Next(100000, 999999).ToString();
        }
    }
}
