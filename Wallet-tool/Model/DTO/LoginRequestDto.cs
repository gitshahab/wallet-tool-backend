using System.ComponentModel.DataAnnotations;

namespace Wallet_tool.Model.DTO
{
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
