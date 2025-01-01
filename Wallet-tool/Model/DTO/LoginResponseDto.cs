namespace Wallet_tool.Model.DTO
{
    public class LoginResponseDto
    {
        public string JwtToken { get; set; }
        public List<String> Role { get; set; }
    }
}
