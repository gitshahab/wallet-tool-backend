using System.ComponentModel.DataAnnotations;

namespace Wallet_tool.Model.DTO
{
    public class RegisterRequestDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters and must contain unique character.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string[] Roles { get; set; }
    }
}
