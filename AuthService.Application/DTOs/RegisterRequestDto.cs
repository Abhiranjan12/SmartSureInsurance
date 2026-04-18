using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.DTOs
{
    public class RegisterRequestDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Range(18, 100)]
        public int Age { get; set; }
    }
}
