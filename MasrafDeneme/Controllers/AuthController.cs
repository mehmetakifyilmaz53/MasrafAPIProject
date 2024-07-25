using MasrafDeneme.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MasrafDeneme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserLogin userLogin1;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
   
        }



        [HttpPost("login")]
        public string Login([FromBody] UserLogin userLogin1)
        {
            if (userLogin1.Username == "test" && userLogin1.Password == "password") // Basit bir kullanıcı doğrulama
            {

                var jwtSettings = _configuration.GetSection("Jwt");

                if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings["Key"]) || string.IsNullOrEmpty(jwtSettings["Issuer"]) || string.IsNullOrEmpty(jwtSettings["Audience"]))
                {
                    throw new ArgumentNullException("JWT settings are not configured correctly in appsettings.json");
                }

                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, userLogin1.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"])),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            return "Unauthorized";
        }
    }
}
