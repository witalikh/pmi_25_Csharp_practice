using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CertificateAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthenticateController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthenticateController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        this._userManager = userManager;
        this._roleManager = roleManager;
        _configuration = configuration;
    }

    [HttpPost]  
    [Route("login")]  
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] Login credentials)  
        {  
            User? user = await _userManager.FindByEmailAsync(credentials.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, credentials.Password))  
                return Unauthorized();  
            
            IList<string>? userRoles = await _userManager.GetRolesAsync(user);
            List<Claim> authClaims = new()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };  

            foreach (string? userRole in userRoles)  
            {  
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));  
            }  

            SymmetricSecurityKey authSigningKey = new(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));  

            JwtSecurityToken token = new(  
                issuer: _configuration["JWT:ValidIssuer"],  
                audience: _configuration["JWT:ValidAudience"],  
                expires: DateTime.Now.AddMinutes(1),  
                claims: authClaims,  
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)  
                );
            return Ok(new  
            {  
                token = new JwtSecurityTokenHandler().WriteToken(token),  
                expiration = token.ValidTo  
            });
        }  
  
        [HttpPost]  
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(Register credentials)  
        {
            User? findUser = await _userManager.FindByEmailAsync(credentials.Email);
            if (findUser != null)  
                return BadRequest(new Dictionary<string, string> {{"detail", "User already exists"}});
            User user = new()
            {  
                FirstName = credentials.FirstName,
                LastName = credentials.LastName,
                Email = credentials.Email,  
                SecurityStamp = Guid.NewGuid().ToString(),  
                UserName = credentials.Username  
            }; 
            
            IdentityResult? result = await _userManager.CreateAsync(user, credentials.Password);
            if (!result.Succeeded)  
                return BadRequest(result.Errors);  
  
            return Ok(User);  
        }  
}