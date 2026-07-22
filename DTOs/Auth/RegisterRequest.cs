using System.ComponentModel.DataAnnotations;

namespace LogisticDesk.DTOs.Auth
{
    public class RegisterRequest
    {
        [Required]
        public required string FullName { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        [MinLength(8)]
        public required string Password { get; set; }
        [Required]
        [Compare(nameof(Password))]
        public required string ConfirmPassword { get; set; }

    }
}
