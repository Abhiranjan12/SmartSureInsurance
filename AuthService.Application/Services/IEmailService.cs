namespace AuthService.Application.Services
{
    public interface IEmailService
    {
        Task SendOtpAsync(string toEmail, string fullName, string otpCode);
    }
}
