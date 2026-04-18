using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateUserDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(Customer|Admin)$", ErrorMessage = "Role must be either 'Customer' or 'Admin'.")]
        public string Role { get; set; } = string.Empty;
    }

    public class UpdateProfileDto
    {
        [Required]
        [MinLength(2)]
        public string FullName { get; set; } = string.Empty;
    }
}
