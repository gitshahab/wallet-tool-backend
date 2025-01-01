using Microsoft.AspNetCore.Identity;

namespace Wallet_tool.Repository
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
