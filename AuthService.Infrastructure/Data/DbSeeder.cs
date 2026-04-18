using AuthService.Domain.Entities;
using AuthService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AuthService.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAdminAsync(AuthDbContext context, IConfiguration config)
        {
            string email = config["AdminSeed:Email"] ?? "abhiranjank037@gmail.com";
            string password = config["AdminSeed:Password"] ?? "Admin@123";
            string fullName = config["AdminSeed:FullName"] ?? "Super Admin";

            bool adminExists = await context.Users.AnyAsync(u => u.Email == email && u.Role == "Admin");
            if (adminExists)
                return;

            var admin = new User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Age = 30,
                Role = "Admin",
                IsEmailVerified = true
            };

            await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();
        }
    }
}
