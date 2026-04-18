using AuthService.Application.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace AuthService.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendOtpAsync(string toEmail, string fullName, string otpCode)
        {
            string? smtpHost = _config["Smtp:Host"];
            if (string.IsNullOrEmpty(smtpHost))
            {
                throw new InvalidOperationException("Smtp:Host is not configured.");
            }

            string? smtpPortValue = _config["Smtp:Port"];
            if (string.IsNullOrEmpty(smtpPortValue))
            {
                throw new InvalidOperationException("Smtp:Port is not configured.");
            }
            int smtpPort = int.Parse(smtpPortValue);

            string? smtpUser = _config["Smtp:Username"];
            if (string.IsNullOrEmpty(smtpUser))
            {
                throw new InvalidOperationException("Smtp:Username is not configured.");
            }

            string? smtpPass = _config["Smtp:Password"];
            if (string.IsNullOrEmpty(smtpPass))
            {
                throw new InvalidOperationException("Smtp:Password is not configured.");
            }

            string? fromEmail = _config["Smtp:From"];
            if (string.IsNullOrEmpty(fromEmail))
            {
                throw new InvalidOperationException("Smtp:From is not configured.");
            }

            string body = "<h2>Hello " + fullName + ",</h2>"
                        + "<p>Your SmartSure verification code is:</p>"
                        + "<h1 style='letter-spacing:8px; color:#2563eb;'>" + otpCode + "</h1>"
                        + "<p>This code expires in <strong>10 minutes</strong>.</p>"
                        + "<p>If you did not request this, please ignore this email.</p>";

            SmtpClient client = new SmtpClient(smtpHost, smtpPort);
            client.Credentials = new NetworkCredential(smtpUser, smtpPass);
            client.EnableSsl = true;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromEmail, "SmartSure");
            mail.Subject = "Your SmartSure OTP Code";
            mail.Body = body;
            mail.IsBodyHtml = true;
            mail.To.Add(toEmail);

            await client.SendMailAsync(mail);
        }
    }
}
