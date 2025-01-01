using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wallet_tool.Model.DTO;
using Wallet_tool.Repository;

namespace Wallet_tool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletAuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public WalletAuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }
        
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]

        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("User was Registered!, Please login");
                    }
                }
            }
            return BadRequest(string.Join(", ", identityResult.Errors.Select(e => e.Description)));
        }
        
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid login details.");
            }
            Console.WriteLine($"Login attempt for user: {loginRequestDto.Username}");
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
            if (user == null)
            {
                return BadRequest("Invalid username or password.");
            }

            var checkPassword = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (!checkPassword)
            {
                return BadRequest("Invalid username or password.");
            }
            var roles = await userManager.GetRolesAsync(user);
            if (roles != null)
            {
                var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                var response = new LoginResponseDto
                {
                    JwtToken = jwtToken,
                    Role = roles.ToList()
                };
                return Ok(response);
            }
            

            return BadRequest("User roles not assigned or invalid.");

        }
    }
}
